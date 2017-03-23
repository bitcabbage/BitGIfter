using Akka.Actor;
using static BitGifter.Core.Actors.PortalActor;
using static BitGifter.Core.Actors.TelegramActor;

namespace BitGifter.Core.Actors
{
    //TODO need actors pool here
    internal class PortalsSupervisorActor : ReceiveActor
    {
        public IActorRef PortalActor { get; set; }
       
        public PortalsSupervisorActor(IActorRef telegramActor)
        {         
            PortalActor = Context.ActorOf(Props.Create(() => new PortalActor(telegramActor)));
            Context.System.EventStream.Subscribe(PortalActor, typeof(NewAuthCookie));          

            //TODO: route this action to the pool
            Receive<BuyGiftCardRequest>(x => ScrapEntityHandler(x));
            Receive<NewAuthCookie>(x => HanldeNewAuthCookie(x));         
        }

        void ScrapEntityHandler(BuyGiftCardRequest entity)
        {
            PortalActor.Forward(entity);
        }

        void HanldeNewAuthCookie(NewAuthCookie cookie)
        {
            PortalActor.Forward(cookie);
        }        

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(x =>
            {
                return Directive.Resume;
            });            
        }
    }
}
