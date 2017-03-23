using System.Threading;
using OpenQA.Selenium;
using System;
using static BitGifter.Core.Actors.PortalActor;

namespace BitGifter.Core.Portal_Wrappers
{
    public class BitcoinInvoice
    {
        public string BitcoinAddress { get; set; }
        public decimal BtcPrice { get; set; }
        public decimal FiatPrice { get; set; }
    }

   

    internal class EgifterPortal : IPortal
    {
        private IWebDriver driver;
        private string baseURL => Environment.GetEnvironmentVariable("GIFT_CARDS_PORTAL");

        public EgifterPortal(IWebDriver driver)
        {
            this.driver = driver;
        }

        public IPortal NavigateTo()
        {
            driver.Navigate().GoToUrl(baseURL);
            return this;
        }

        public IPortal SetAuthCookie(string cookieValue)
        {
            var cookie = new Cookie("ega-GA", cookieValue);
            driver.Manage().Cookies.DeleteAllCookies();
            driver.Manage().Cookies.AddCookie(cookie);
            return this;
        }

        public bool IsAuthenticated
        {
            get
            {
                var url = driver.Url;
                GoToGiftCards();
                return !(driver.Url == "https://www.BitGifter.com/giftcards/?auth=1&se=1");
            }
        }

        public IPortal GoToGiftCards()
        {
            driver.Navigate().GoToUrl(baseURL + "/giftcards/");
            return this;
        }

        public IPortal ChooseCard(BuyGiftCardRequest request)
        {
            driver.FindElement(By.XPath($"//img[@alt='{request.GiftCard.Description}']")).Click();
            return this;
        }

        public IPortal SetCardPrice(BuyGiftCardRequest request)
        {
            driver.FindElement(By.Name("giftGoal")).Clear();
            driver.FindElement(By.Name("giftGoal")).SendKeys(request.GiftCard.Price.ToString());

            Thread.Sleep(1000);
            driver.FindElement(By.Id("btnSelf")).Click();
            return this;
        }

        public IPortal Checkout()
        {
            driver.FindElement(By.LinkText("Checkout")).Click();
            return this;
        }

        public IPortal PayWithBitcoin(Action<BitcoinInvoice> paymentCallback)
        {
            driver.FindElement(By.CssSelector("img.pay-with-btc")).Click();

            driver.FindElement(By.Id("copy-tab")).Click();
            var elem = driver.FindElement(By.CssSelector("div.manual-box__address__wrapper__value"));
            var bitcoinAddress = elem.Text;

            var btcPrice = driver.FindElement(By.CssSelector("div.single-item-order__right__btc-price")).Text;
            var fiatPrice = driver.FindElement(By.CssSelector("div.single-item-order__right__fiat-price")).Text;
            var invoice = new BitcoinInvoice
            {
                BitcoinAddress = bitcoinAddress,
                BtcPrice = Convert.ToDecimal(btcPrice),
                FiatPrice = Convert.ToDecimal(fiatPrice)
            };

            paymentCallback(invoice);

            return this;
        }

        public string GetCardCode()
        {
            throw new NotImplementedException();
        }

        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
