namespace BitGifter.Core.Invoices
{
    public class BitcoinInvoice
    {
        public string BitcoinAddress { get; set; }
        public decimal BtcPrice { get; set; }
        public decimal FiatPrice { get; set; }
    }
}
