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
    class TabidooDataSender : IDataSender
    {
        private const int GonnaSend = 1000;

        private readonly string _url;
        private readonly string _appId;
        private readonly string _token;

        public TabidooDataSender(string url, string appId, string token)
        {
            _url = url;
            _appId = appId;
            _token = token;
        }

        public async Task SendItems<T>(string schemaId, List<T> items) where T: Item
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            string url = $"{_url}/apps/{_appId}/schemas/{schemaId}/data/bulk?dataResponseType=All";
            int itemsToSkip = 0;
            int successfulSent = 0;

            List<string> errorMessages = new List<string>();

            while (items.Count > 0)
            {
                var json = GetJsonList(items, itemsToSkip, out int readItems);

                itemsToSkip += readItems;

                try
                {
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        ProcessResponse(response, items, ref successfulSent, errorMessages);
                    }
                }
                catch (Exception e)
                {
                    throw new LocalException("TabidooDataSender", 
                        "An error occurred while sending items", e.Message);
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

        private void ProcessResponse<T>(HttpResponseMessage response, List<T> items, 
            ref int successfulSent, List<string> errorMessages) where T: Item
        {
            var jObject = JObject.Parse(response.Content.ToString() ?? string.Empty);
            successfulSent += (int) (jObject["bulk"]?["successCount"] ?? 0);
            var serializedItems = jObject["data"]?.Children() ?? new JEnumerable<JToken>();

            foreach (var serializedItem in serializedItems)
            {
                var deserializedItem = JsonConvert.DeserializeObject<T>(serializedItem["fields"]?.ToString() 
                                                                        ?? string.Empty);
                var localItem = items.Single(i => i == deserializedItem);
                localItem.Id = serializedItem["id"]?.ToString();
            }

            errorMessages.AddRange(jObject["errors"]?.Children()["message"].Select(m => m.ToString()).ToList() 
                                   ?? new List<string>());
        }
    }
}
