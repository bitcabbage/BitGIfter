using Akka.Actor;
using Akka.Event;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace BitGifter.Core.Actors
{
    internal class TelegramActor : ReceiveActor
    {
        #region Messages

        public class RequestCookie { }
        public class NewAuthCookie
        {
            public string Value { get; set; }
        }

        #endregion

        #region State

        public long ChatId = 101333219;
        static EventStream _stream;
        Telegram.Bot.TelegramBotClient _bot;

        #endregion

        public TelegramActor()
        {
            _stream = Context.System.EventStream;
            _bot = BuildBot();
            Receive<RequestCookie>(x => HanldeRequestCookie(x));
        }

        #region Private Helpers

        private TelegramBotClient BuildBot()
        {
            var bot = new Telegram.Bot.TelegramBotClient("340012648:AAF1bURuuTS7Dce5iDfkQrINRDHNMftSSeo");
            bot.OnMessage += BotOnMessageReceived;
            bot.StartReceiving();
            return bot;
        }

        private void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.TextMessage && message.Chat.Id != ChatId) return;

            if (message.Text.StartsWith("/cookie"))
            {
                _stream.Publish(new NewAuthCookie() { Value = message.Text.Replace("/cookie ", string.Empty) });
            }
            else
            {
                var usage = @"Usage:
                    ///cookie <cookie value>   - send cookie";

                _bot.SendTextMessageAsync(message.Chat.Id, usage);
            }
        }

        private void HanldeRequestCookie(RequestCookie x)
        {
            _bot.SendTextMessageAsync(this.ChatId, "Auth cookie required");
        }

        #endregion
    }
}
