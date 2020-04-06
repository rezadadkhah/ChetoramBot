using Business.Business.User;
using Business.Common.Extentions;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChetoramBot
{
    public static class Application
    {
        private static TelegramBotClient botClient;
        public static void Run()
        {
            botClient = new TelegramBotClient("1010447647:AAErBW8AbSk5y5V_XhQ2i1djhKB6dDT9Epo");
            botClient.OnCallbackQuery += OnCallbackQuery;
            botClient.OnMessage += OnMessage;
            botClient.StartReceiving();
            Console.ReadLine();
        }
        private static async void OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            string[] data = e.CallbackQuery.Data.Split("-");
            UserServey userServey = new UserServey()
            {
                VoterUserId = int.Parse(data[0],null),
                ConsideredUserId = int.Parse(data[1],null),
                ServeyId = int.Parse(data[2],null),
                Point = int.Parse(data[3],null),
                SurveyDate = DateTime.Now
            };
            InsertUserServey insertUserServey = new InsertUserServey(userServey);
            insertUserServey.Run();

            botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id);
            botClient.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId, e.CallbackQuery.Message.Text + " => " + data[4]);
        }
        private static async void OnMessage(object sender, MessageEventArgs e)
        {
            await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: Messages.StartClient,
                            replyMarkup: CreateMarkupKeyboardButtons()
                        ).ConfigureAwait(false);
            if (e.Message.Type == MessageType.Text)
                ProcessText(e);
        }

        private static async Task ProcessText(MessageEventArgs e)
        {
            if (e.Message.Text == "/start")
            {
                StartClient(e);
                return;
            }
            if (e.Message.Text == "لینک نظردهی ناشناس من")
            {
                GetPrivateLink(e);
                return;
            }
            if (e.Message.Text.StartsWith("/start") &&
                e.Message.Text.Split(" ")[0] == "/start" &&
                !e.Message.Text.Split(" ")[1].IsNullOrEmptyOrWhitespace() &&
                e.Message.Text.Split(" ")[1].Split("-")[0] == "PL" &&
                !e.Message.Text.Split(" ")[1].Split("-")[1].IsNullOrEmptyOrWhitespace() &&
                int.TryParse(e.Message.Text.Split(" ")[1].Split("-")[1], out int userId))
            {
                GetNewServey(e, userId);
                return;
            }
        }

        private static void GetNewServey(MessageEventArgs e, int userId)
        {
            if(CheckSurveyPermission(e,userId) == false)
            {
                return;
            }
            GetServey getServey = new GetServey();
            getServey.Run();

            CreateAndSendServeyInlineKeyboard(e, userId, getServey.Result);

        }

        private static bool CheckSurveyPermission(MessageEventArgs e, int userId)
        {
            if(e.Message.From.Id == userId)
            {
                botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: "شما نمیتونید به خودتون رای بدید  :)"
                        ).ConfigureAwait(false);
                return false;
            }
            CheckLastSurveyDate checkLastSurveyDate = new CheckLastSurveyDate(e.Message.From.Id,userId);
            checkLastSurveyDate.Run();
            if(checkLastSurveyDate.Result == false)
            {
                 botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: "باید از آخرین رای شما به این کاربر 24 ساعت بگذره بعد رای بدید :)"
                        ).ConfigureAwait(false);
                return false;
            }
            return true;
        }

        private static void CreateAndSendServeyInlineKeyboard(MessageEventArgs e, int userId, List<Survey> serveys)
        {
            foreach (Survey survey in serveys)
            {

                botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: survey.PersianTitle,
                            replyMarkup: new InlineKeyboardMarkup(new[]
                        {
                            InlineKeyboardButton.WithCallbackData("نه اصلا",e.Message.From.Id + "-" + userId + "-" + survey.Id + "-0-نه اصلا"),
                            InlineKeyboardButton.WithCallbackData("خیلی کم",e.Message.From.Id + "-" + userId + "-" + survey.Id + "-25-خیلی کم"),
                            InlineKeyboardButton.WithCallbackData("متوسط",e.Message.From.Id + "-" + userId + "-" + survey.Id + "-50-متوسط"),
                            InlineKeyboardButton.WithCallbackData("آره",e.Message.From.Id + "-" + userId + "-" + survey.Id + "-75-آره"),
                            InlineKeyboardButton.WithCallbackData("صد درصد",e.Message.From.Id + "-" + userId + "-" + survey.Id + "-100-صد درصد")
                        })
                        ).ConfigureAwait(false);

            }
        }

        private static async Task GetPrivateLink(MessageEventArgs e)
        {
            await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: Messages.BotURL + "PL-" + e.Message.From.Id
                        ).ConfigureAwait(false);
        }

        private static async Task StartClient(MessageEventArgs e)
        {
            Telegram.Bot.Types.User user = e.Message.From;
            CreateUser CreateUser = new CreateUser(new DataAccess.Models.User
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                lastName = user.LastName
            });
            CreateUser.Run();
            await botClient.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: Messages.StartClient,
                            replyMarkup: CreateMarkupKeyboardButtons()
                        ).ConfigureAwait(false);
        }

        private static ReplyKeyboardMarkup CreateMarkupKeyboardButtons()
        {
            return new ReplyKeyboardMarkup(
               new[]
                    {
                        new[]
                        {
                            new KeyboardButton(":)" + ":-)")
                        },
                        new[]
                        {
                            KeyboardButton.WithRequestContact(":D" + ":-D")
                        }
                    }
            );
        }



    }
}
