using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BitGifter.Core.Cross_Cutting
{
    public class HttpClientWrapper
    {
        public HttpClient _client { get; set; } = new HttpClient();
        public string _resource { get; set; } = string.Empty;
        public object _param { get; set; }

        public HttpClientWrapper SetBaseAddress(string baseAddress)
        {
            _client.BaseAddress = new Uri(baseAddress);
            return this;
        }

        public HttpClientWrapper SetResource(string resource)
        {
            _resource = resource;
            return this;
        }
        public HttpClientWrapper SetAuthHeader(string token)
        {
            _client.DefaultRequestHeaders.Add("x-api-key", token);
            return this;
        }

        public HttpClientWrapper AddParameter<T>(T parameter) where T : class
        {
            _param = parameter;
            return this;
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync()
        {
            var jsonString = JsonConvert.SerializeObject(_param);
            return await _client.PostAsync(_client.BaseAddress + _resource, new StringContent(jsonString, Encoding.UTF8, "application/json"));
        }
    }
}
