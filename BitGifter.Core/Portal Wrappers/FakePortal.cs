using System.Threading;
using OpenQA.Selenium;
using System;
using static BitGifter.Core.Actors.PortalActor;

namespace BitGifter.Core.Portal_Wrappers
{
    internal class FakePortal: IPortal
    {
        private string _cookieValue = "initial";
        private string baseURL => Environment.GetEnvironmentVariable("GIFT_CARDS_PORTAL");
        private int _sleepInterval = 2000;

        public FakePortal(IWebDriver driver)
        {
            //this.driver = driver;
        }

        public IPortal NavigateTo()
        {
            Thread.Sleep(_sleepInterval);
            Console.WriteLine($"navigate to {baseURL}");
           
            return this;
        }

        public IPortal SetAuthCookie(string cookieValue)
        {
            Thread.Sleep(_sleepInterval);
            Console.WriteLine($"setting cookie value to ${cookieValue}");
            _cookieValue = cookieValue;      
            return this;
        }

        public bool IsAuthenticated
        {
            get
            {
                return _cookieValue == "valid";                
            }
        }

        public IPortal GoToGiftCards()
        {
            Thread.Sleep(_sleepInterval);
            Console.WriteLine($"go to giftcards");
            return this;
        }

        public IPortal ChooseCard(BuyGiftCardRequest request)
        {
            Thread.Sleep(_sleepInterval);
            Console.WriteLine($"choose card {request.GiftCard.Description}"); 
            return this;
        }

        public IPortal SetCardPrice(BuyGiftCardRequest request)
        {
            Thread.Sleep(_sleepInterval);
            Console.WriteLine($"set card price {request.GiftCard.Price}");
            return this;
        }

        public IPortal Checkout()
        {
            Thread.Sleep(_sleepInterval);
            Console.WriteLine($"go to checkout");
            return this;
        }

        public IPortal PayWithBitcoin(Action<BitcoinInvoice> paymentCallback)
        {
            Thread.Sleep(_sleepInterval);
            Console.WriteLine($"pay with bitcoin");

            var invoice = new BitcoinInvoice
            {
                BitcoinAddress = Guid.NewGuid().ToString(),
                BtcPrice = 1,
                FiatPrice = 700
            };

            paymentCallback(invoice);

            return this;
        }

        public string GetCardCode()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
