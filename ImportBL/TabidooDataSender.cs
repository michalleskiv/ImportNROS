using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ImportBL.Exceptions;
using ImportBL.Interfaces;
using ImportBL.Models;
using ImportBL.Models.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ImportBL
{
    public class TabidooDataSender : IDataSender
    {
        private const int GonnaSend = 1000;

        private readonly string _url;
        private readonly string _appId;
        private readonly string _token;
        private readonly ILogger _logger;

        public TabidooDataSender(IConfiguration configuration, ILogger logger)
        {
            _url = configuration.Url;
            _appId = configuration.AppId;
            _token = configuration.Token;
            _logger = logger;
        }

        public async Task SendItems<T>(string schemaId, List<T> items) where T: Item
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            string url = $"{_url}/apps/{_appId}/schemas/{schemaId}/data/bulk?dataResponseType=All";
            int itemsToSkip = 0;
            int successfulSent = 0;

            while (items.Count > itemsToSkip)
            {
                var json = GetJsonList(items, itemsToSkip, out int readItems);

                itemsToSkip += readItems;

                try
                {
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        successfulSent = await ProcessResponse(response, items, successfulSent);
                    }
                }
                catch (LocalException e)
                {
                    _logger.LogException(e);
                }
                catch (Exception e)
                {
                    _logger.LogException(e);
                }
            }
        }

        public async Task UpdateContacts(string contactsSchemaId, List<Contact> contacts)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            foreach (var contact in contacts)
            {
                string url = $"{_url}/apps/{_appId}/schemas/{contactsSchemaId}/data/{contact.Id}";
                var contactToSend = new UpdateContactHolder
                {
                    Fields = new ContactUpdateDto(contact)
                };
                var serializedContact = JsonConvert.SerializeObject(contactToSend, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    },
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                });
                var content = new StringContent(serializedContact, Encoding.UTF8, "application/json");

                var response = await httpClient.PatchAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogException(contact + "\nwas updated");
                }
            }
        }

        private string GetJsonList<T>(List<T> items, int itemsToSkip, out int readItems) where T: Item
        {
            var dataToSend = items
                .Skip(itemsToSkip)
                .Take(GonnaSend)
                .Select(i => new ItemHolder<T>
                    {
                        Fields = i
                    })
                .ToList();

            readItems = dataToSend.Count;

            var obj = new ItemsHolder<T>
            {
                Bulk = dataToSend
            };
            
            var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                },
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            return json;
        }

        private async Task<int> ProcessResponse<T>(HttpResponseMessage response, List<T> items, 
            int successfulSent) where T: Item
        {
            var jObject = JObject.Parse(await response.Content.ReadAsStringAsync() ?? string.Empty);
            successfulSent += (int) (jObject["bulk"]?["successCount"] ?? 0);
            var serializedItems = jObject["data"]?.Children() ?? new JEnumerable<JToken>();

            foreach (var serializedItem in serializedItems)
            {
                var deserializedItem = JsonConvert.DeserializeObject<T>(serializedItem["fields"]?.ToString() 
                                                                        ?? string.Empty);
                var localItem = items.SingleOrDefault(i => i.Equals(deserializedItem));

                if (localItem != null)
                {
                    localItem.Id = serializedItem["id"]?.ToString();
                }
            }

            var serializedErrors = jObject["errors"]?.Children() ?? new JEnumerable<JToken>();

            foreach (var error in serializedErrors)
            {
                var erroneousItem = error["recordIndex"] != null ? items[(int) error["recordIndex"]] : null;

                var errorToWrite = $"{error}\n" + erroneousItem;
                _logger.LogException(errorToWrite);
            }

            return successfulSent;
        }
    }
}
