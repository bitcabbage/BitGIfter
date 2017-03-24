namespace BitGifter.Core.BitWallet.Messages
{
    public class WalletResponse
    {
        public class Wallet
        {
            public string address { get; set; }
        }

        public Wallet wallet { get; set; }
    }   
}
