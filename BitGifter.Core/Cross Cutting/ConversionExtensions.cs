namespace BitGifter.Core.Cross_Cutting
{
    public static class ConversionExtensions
    {
        public static decimal BtcToSatoshi(this decimal btc)
        {
            return btc * 100_000_000;
        }
    }
}
