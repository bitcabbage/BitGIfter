using BitGifter.Core.BitWallet.Messages;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BitGifter.Core.BitWallet
{
    public class WalletService
    {
        protected string BitWalletAddress => Environment.GetEnvironmentVariable("BIT_WALLET_ADDRESS");
        protected string BitWalletAuthHeader => Environment.GetEnvironmentVariable("BIT_WALLET_AUTH_HEADER");

        public WalletResponse CreateWallet(WalletRequest request)
        {
            var response = new HttpClientWrapper()
             .SetAuthHeader(BitWalletAuthHeader)
             .SetBaseAddress(BitWalletAddress)
             .SetResource("/wallets")
             .AddParameter(request)
             .PostAsJsonAsync().Result;

            //if (!response.IsSuccessStatusCode)
            //{
            //    return response.StatusCode.ToString();
            //}
            var responseJson = response.Content.ReadAsStringAsync().Result.ToString();


            var res = JsonConvert.DeserializeObject<WalletResponse>(responseJson);

            return res;
        }

        public PaymentResponse MakePayment(PaymentRequest request)
        {
            var response = new HttpClientWrapper()
             .SetAuthHeader(BitWalletAuthHeader)
             .SetBaseAddress(BitWalletAddress)
             .SetResource("/payments")
             .AddParameter(request)
             .PostAsJsonAsync().Result;

            //if (!response.IsSuccessStatusCode)
            //{
            //    return response.StatusCode.ToString();
            //}
            var responseJson = response.Content.ReadAsStringAsync().Result.ToString();


            var res = JsonConvert.DeserializeObject<PaymentResponse>(responseJson);

            return res;
        }
    }
  
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
