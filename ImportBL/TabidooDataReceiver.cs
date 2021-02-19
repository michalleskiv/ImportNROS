using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ImportBL.Interfaces;
using ImportBL.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ImportBL
{
    public class TabidooDataReceiver : IDataReceiver
    {
        private const int GonnaRead = 1000;

        private readonly string _url;
        private readonly string _appId;
        private readonly string _token;
        private readonly ILogger _logger;

        public TabidooDataReceiver(string url, string appId, string token, ILogger logger)
        {
            _url = url;
            _appId = appId;
            _token = token;
            _logger = logger;
        }

        public async Task<List<T>> GetTable<T>(string schemaId) where T: Item
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            string url = $"{_url}/apps/{_appId}/schemas/{schemaId}/data";

            var items = new List<T>();

            int readCount = GonnaRead;

            for (int i = 0; readCount > 0; i += readCount)
            {
                try
                {
                    readCount = 0;
                    var result = await httpClient.GetAsync(url + $"?limit={GonnaRead}&skip={i}");

                    if (result.IsSuccessStatusCode)
                    {
                        var content = await result.Content.ReadAsStringAsync();
                        var jObject = JObject.Parse(content);
                        var serializedContacts = jObject["data"]?.Children() ?? new JEnumerable<JToken>();

                        foreach (var serializedContact in serializedContacts)
                        {
                            var deserializedObject = JsonConvert.DeserializeObject<T>(
                                serializedContact["fields"]?.ToString()
                                ?? string.Empty);
                            deserializedObject.Id = serializedContact["id"]?.ToString();
                            items.Add(deserializedObject);
                            readCount++;
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogException(e);
                }
            }

            return items;
        }
    }
}
