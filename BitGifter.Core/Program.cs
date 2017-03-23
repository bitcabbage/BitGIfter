using Akka.Actor;
using BitGifter.Core.Actors;
//using BitGifter.Core.BitWallet;
using BitGifter.Core.Customers;
using BitGifter.Core.Gift_Cards;
using System;
using System.Collections.Generic;
using System.IO;
using static BitGifter.Core.Actors.PortalActor;

namespace BitGifter.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Running eGifter in {Directory.GetCurrentDirectory()}");           

            var system = PortalActorSystem.Build();

            var requests = new List<BuyGiftCardRequest>()
            {
                new BuyGiftCardRequest
                {
                    Customer = new Customer { Id = "artur_test" },
                    GiftCard = new GiftCard { Price = 10, Description = "Amazon.com Gift Card" }
                },
                new BuyGiftCardRequest
                {
                    Customer = new Customer { Id = "artur_test" },
                    GiftCard = new GiftCard { Price = 15, Description = "Amazon.com Gift Card" }
                },
            };

            requests.ForEach(r => system.RootActor.Tell(r));
            system.System.WhenTerminated.Wait();
        }       
    }
}
