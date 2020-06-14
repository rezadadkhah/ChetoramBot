using ChetoramBot.Helpers;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
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
            BotClient.OnMessage += OnMessage;
            BotClient.OnCallbackQuery += OnCallbackQuery;
            BotClient.StartReceiving();
        }

        private static void OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Type == MessageType.Text)
                ProcessText(e);
        }

        private static void OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            ProcessCallbackQuery(e);
        }

        public static void ProcessCallbackQuery(CallbackQueryEventArgs e)
        {
            if (null == e) return;
            string startsWith = e.CallbackQuery.Data.Split("_")[0];
            switch (startsWith)
            {
                case "MyPL":
                {
                    Statics.GetPrivateLink(e);
                    break;
                }
                case "MyReport":
                {
                    Statics.GetReport(e);
                    break;
                }
                case "MainMenu":
                {
                    Statics.GetMainMenu(e.CallbackQuery.Message);
                    break;
                }
                case "Survey":
                {
                        Statics.SetVote(e);
                        break;
                }
                case "SkipSurvey":
                {
                    Statics.SkipVote(e);
                    break;
                }
                default:
                {
                    Statics.SetVote(e);
                    break;
                }
            }
        }

        private static void ProcessText(MessageEventArgs e)
        {
            if (e.Message.Text == "/start")
            {
                Statics.StartClient(e.Message);
                return;
            }
            if (Statics.CheckIsSurvey(e, out int userId))
            {
                Statics.SendNewSurvey(e, userId);
            }
        }
    }
}
