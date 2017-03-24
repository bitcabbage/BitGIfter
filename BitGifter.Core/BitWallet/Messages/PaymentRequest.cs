namespace BitGifter.Core.BitWallet.Messages
{
    public class PaymentRequest
    {
        #region Inner Classes

        public class Transfer
        {
            public string to { get; set; }
            public decimal amount { get; set; }
        }

        public class Customer
        {
            public string id { get; set; }
        }

        #endregion

        public Customer customer { get; set; }
        public Transfer transfer { get; set; }
    }
}
