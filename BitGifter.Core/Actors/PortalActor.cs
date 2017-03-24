using Akka.Actor;
using Akka.Event;
using BitGifter.Core.BitWallet;
using BitGifter.Core.BitWallet.Messages;
using BitGifter.Core.Customers;
using BitGifter.Core.Gift_Cards;
using BitGifter.Core.Portal_Wrappers;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.IO;
using static BitGifter.Core.Actors.TelegramActor;


namespace BitGifter.Core.Actors
{
    public class PortalActor : ReceiveActor, IWithUnboundedStash
    {
        #region Messages

        public class BuyGiftCardRequest
        {
            public Customer Customer { get; set; }
            public GiftCard GiftCard { get; set; }
            public string Id { get; set; } = Guid.NewGuid().ToString();
        }

        #endregion

        #region State

        public IStash Stash { get; set; }
        private IPortal _portal;
        private NewAuthCookie _authCookie = new NewAuthCookie
        {
            Value = "065132008093095216224064103177182057142204051159022169008132059008229133025213171183197030249011014208128179180246000109165083041034160064129013086152197010047059165078046136044121060081151036092190165013147121100240190214223034226037169203251248199141254079126079240025247052174031135115228086098199090216147195245142035155017225181248232088237040169183091167164238120108139238240074067131214118096151179205171214163254004245166254053044121123010222168123168109095087033025067105"
        };

        private IWebDriver driver;
        private readonly ILoggingAdapter _log = Logging.GetLogger(Context);
        private IActorRef _telegramActor;

        #endregion

        #region Handlers

        void HandleNewAuthCookie(NewAuthCookie x)
        {
            _authCookie.Value = x.Value;
            _portal.SetAuthCookie(x.Value);
            Become(Authenticated);
        }

        void BuyGiftCardHandler(BuyGiftCardRequest request)
        {
            _log.Info($"Handling request. Id: {request.Id}. Price: {request.GiftCard.Price}. Description: {request.GiftCard.Description}");

            if (!_portal.IsAuthenticated)
            {
                _log.Warning("Auth required");

                _telegramActor.Tell(new RequestCookie());

                Become(UnAuthenticated);
            }
            else
            {
                var cardCode = _portal
                      .GoToGiftCards()
                      .ChooseCard(request)
                      .SetCardPrice(request)
                      .Checkout()
                      .PayWithBitcoin(invoice =>
                      {
                          _log.Info($"payment callback. wallet: {invoice.BitcoinAddress}, btc : {invoice.BtcPrice}, fiat: {invoice.FiatPrice} ");

                          var walletService = new WalletService();

                          var waletResponse = walletService.CreateWallet(new WalletRequest { customer = new WalletRequest.Customer { id = request.Customer.Id } });
                          var paymentResult = walletService.MakePayment(new PaymentRequest { customer = new PaymentRequest.Customer { id = request.Customer.Id }, transfer = new PaymentRequest.Transfer { amount = invoice.SatoshiPrice, to = invoice.BitcoinAddress } });                       
                      })
                      .GetCardCode();

                _log.Info($"BuyCardHandler finished. Card code: {cardCode}");
            }
            _log.Info($"BuyCardHandler finished.");
        }

        #endregion

        protected override void PreStart()
        {
            Setup();
            base.PreStart();
        }

        protected override void PostStop()
        {
            driver.Dispose();
            base.PostStop();
        }

        protected override void PreRestart(Exception reason, object message)
        {
            if (null != message)
            {
                Setup();
            }
        }

        public PortalActor(IActorRef telegramActor)
        {
            _telegramActor = telegramActor;
            Authenticated();
        }

        private void Authenticated()
        {
            _log.Info("Become Authenticated");

            if (null != Stash)
            {
                Stash.UnstashAll();
            }

            Receive<BuyGiftCardRequest>(x => BuyGiftCardHandler(x));
            Receive<NewAuthCookie>(x => HandleNewAuthCookie(x));
        }

        private void UnAuthenticated()
        {
            _log.Info("Become Unauthenticated");
            Stash.Stash();
            Receive<BuyGiftCardRequest>(x => Stash.Stash());
            Receive<NewAuthCookie>(x => HandleNewAuthCookie(x));
        }

        #region Init Actor

        private IWebDriver BuildFireFoxDriver()
        {
            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(Directory.GetCurrentDirectory(), "geckodriver.exe");
            service.FirefoxBinaryPath = Directory.GetCurrentDirectory();
            // var driver = new FirefoxDriver(service, new FirefoxProfile(), TimeSpan.FromSeconds(10));
            var driver = new FirefoxDriver(service);
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            return driver;
        }

        private IWebDriver BuildChromeDriver()
        {
            var driver = new ChromeDriver(Directory.GetCurrentDirectory());
            //driver = new ChromeDriver("c:\\temp");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            return driver;
        }

        private void Setup()
        {
            //driver = BuildFireFoxDriver();
            this.driver = BuildChromeDriver();
            // this.portal = new EgifterPortal(driver);
            this._portal = new FakePortal(driver);
            _portal.NavigateTo();
            _portal.SetAuthCookie(_authCookie.Value);
        }

        #endregion

    }
}
