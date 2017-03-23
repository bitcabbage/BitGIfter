using Akka.Actor;
using BitGifter.Core.Actors;

namespace BitGifter.Core.Actors
{
    public class PortalActorSystem
    {
        public Akka.Actor.ActorSystem System;
        public IActorRef RootActor;
        public IActorRef TelegramActor;

        public static PortalActorSystem Build()
        {
            var system = Akka.Actor.ActorSystem.Create("eGifter");

            var telegramActor = system.ActorOf(Props.Create(() => new TelegramActor()), "telegram");
            var rootActor = system.ActorOf(Props.Create(() => new PortalsSupervisorActor(telegramActor)), "portalActors");

            return new PortalActorSystem
            {
                System = system,
                RootActor = rootActor,
                TelegramActor = telegramActor
            };
        }
    }
}