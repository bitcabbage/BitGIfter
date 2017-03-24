namespace BitGifter.Core.BitWallet.Messages
{
    public class WalletRequest
    {
        #region Inner Classes

        public class Customer
        {
            public string id { get; set; }
        }

        #endregion

        public Customer customer { get; set; }
    }
}
