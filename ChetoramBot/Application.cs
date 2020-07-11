using ChetoramBot.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ChetoramBot
{
    public static class Application
    {
        public static TelegramBotClient BotClient;
        public static void Run()
        {
            Initialize();
            Thread.Sleep(int.MaxValue);
        }

        private static void Initialize()
        {
            BotClient = new TelegramBotClient("1010447647:AAErBW8AbSk5y5V_XhQ2i1djhKB6dDT9Epo");
            CancellationTokenSource cts = new CancellationTokenSource();
            BotClient.StartReceiving(
               new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync),
               cts.Token
            );
        }
        public static async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(update.Message),
                UpdateType.EditedMessage => BotOnMessageReceived(update.Message),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(update.CallbackQuery),
                _ => throw new Exception()
            };

            try
            {
                await handler.ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(exception, cancellationToken).ConfigureAwait(false);
            }
        }
        private static async Task BotOnMessageReceived(Message message)
        {
            if (message.Type == MessageType.Text)
                _ = ProcessText(message);
        }
        public static async Task HandleErrorAsync(Exception exception, CancellationToken cancellationToken)
        {
        }
        

        private static async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery)
        {
            if (null == callbackQuery) return;
            string startsWith = callbackQuery.Data.Split("_")[0];
            switch (startsWith)
            {
                case "MyPL":
                    {
                        Statics.GetPrivateLink(callbackQuery);
                        break;
                    }
                case "MyReport":
                    {
                        Statics.GetReport(callbackQuery);
                        break;
                    }
                case "MainMenu":
                    {
                        Statics.GetMainMenu(callbackQuery.Message);
                        break;
                    }
                case "Survey":
                    {
                        Statics.SetVote(callbackQuery);
                        break;
                    }
                case "SkipSurvey":
                    {
                        Statics.SkipVote(callbackQuery);
                        break;
                    }
                default:
                    {
                        Statics.SetVote(callbackQuery);
                        break;
                    }
            }
        }

        private static async Task ProcessText(Message message)
        {
            if (message.Text == "/start")
            {
                Statics.StartClient(message);
                return;
            }
            int userId = 0;
            if (Statics.CheckIsSurvey(message, userId))
            {
                Statics.SendNewSurvey(message, userId);
            }
        }
    }
}
