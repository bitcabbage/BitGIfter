using System;
using static BitGifter.Core.Actors.PortalActor;

namespace BitGifter.Core.Portal_Wrappers
{
    internal interface IPortal
    {
        bool IsAuthenticated { get; }

        IPortal Checkout();
        IPortal ChooseCard(BuyGiftCardRequest reqest);
        IPortal GoToGiftCards();
        IPortal NavigateTo();
        IPortal PayWithBitcoin(Action<BitcoinInvoice> paymentCallback);
        IPortal SetAuthCookie(string cookieValue);
        IPortal SetCardPrice(BuyGiftCardRequest request);
        string GetCardCode();
    }
}
