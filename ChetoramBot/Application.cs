using Business.Business.Survey;
using ChetoramBot.Helpers;
using DataAccess.Models;
using System.Threading;
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
            BotClient.OnUpdate += OnUpdate;
            BotClient.OnCallbackQuery += OnCallbackQuery;
            BotClient.StartReceiving();
        }

        private static void OnUpdate(object? sender, UpdateEventArgs e)
        {
            
        }

        private static void OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Type == MessageType.Text)
                ProcessText(e); 
        }

        private static void OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            Statics.ProcessCallbackQuery(e);


            
        }



        private static void ProcessText(MessageEventArgs e)
        {
            if (e.Message.Text == "/start")
            {
                Statics.StartClient(e);
                return;
            }
            if (e.Message.Text == "لینک نظردهی ناشناس من")
            {
                //Statics.GetPrivateLink(e);
                return;
            }

            if (Statics.CheckIsSurvey(e, out int userId))
            {
                SendNewSurvey(e, userId);
            }
        }

        private static async Task GetPrivateLink(UpdateEventArgs e)
        {
            GetSurvey getSurvey = new GetSurvey();
            getSurvey.Run();
            Statics.CreateAndSendSurveyInlineKeyboard(e, userId, getSurvey.Result);
        }
    }
}
