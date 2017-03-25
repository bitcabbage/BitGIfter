using BitGifter.Core.BitWallet.Messages;
using BitGifter.Core.Cross_Cutting;
using Newtonsoft.Json;
using System;

namespace BitGifter.Core.BitWallet
{
    public class WalletService : IWalletService
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
  
}
