using BitGifter.Core.BitWallet.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitGifter.Core.BitWallet
{
    public class FakeWalletService : IWalletService
    {
        protected string BitWalletAddress => Environment.GetEnvironmentVariable("BIT_WALLET_ADDRESS");
        protected string BitWalletAuthHeader => Environment.GetEnvironmentVariable("BIT_WALLET_AUTH_HEADER");

        public WalletResponse CreateWallet(WalletRequest request)
        {
            return new WalletResponse
            {
                wallet = new WalletResponse.Wallet
                {
                    address = Guid.NewGuid().ToString()
                }
            };
        }

        public PaymentResponse MakePayment(PaymentRequest request)
        {
            return new PaymentResponse
            {

            };
        }
    }

}
