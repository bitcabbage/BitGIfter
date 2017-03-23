using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Akka.Actor;
using BitGifter.Core.Actors;
using static BitGifter.Core.Actors.PortalActor;

namespace BitGifter.API.Controllers
{
    public class BuyGiftCardResponse
    {
        public string CardCode { get; set; }
    }

    [Route("api/[controller]")]
    public class GiftCardsController : Controller
    {
        public GiftCardsController(PortalActorSystem actorSystem)
        {
            this.ActorSystem = actorSystem;
        }

        public readonly PortalActorSystem ActorSystem;

        [HttpPost("[action]")]
        public BuyGiftCardResponse Buy([FromBody]BuyGiftCardRequest request)
        {
            ActorSystem.RootActor.Tell(request);
            //todo figure out response
            return new BuyGiftCardResponse
            {
                CardCode = Guid.NewGuid().ToString()
            };
        }
    }
}
