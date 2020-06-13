using System;
using Business.Business.User;
using Business.Common.Extentions;
using DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Helpers;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Business.Models;

namespace ChetoramBot.Helpers
{
    public static class Statics
    {
        public static InlineKeyboardMarkup GetMainMenu()
        {
            List<List<InlineKeyboardButton>> buttons = new List<List<InlineKeyboardButton>>();
            List<InlineKeyboardButton> firstRow = new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("لینک نظردهی ناشناس من", "MyPL")
            };
            List<InlineKeyboardButton> secondRow = new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("دیدن نظرات دیگران در مورد خودم", "MyReport")
            };
            List<InlineKeyboardButton> thirdRow = new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("منوی اصلی", "MainMenu")
            };
            buttons.Add(firstRow);
            buttons.Add(secondRow);
            buttons.Add(thirdRow);
            return new InlineKeyboardMarkup(buttons);
        }

        public static IReplyMarkup GetSurveyButtons(Message message, int consideredUserId, DataAccess.Models.Survey survey)
        {
            if (message == null || survey == null)
                return null;
            return new InlineKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("نه اصلا",SerializeSurveyButtonData(message, consideredUserId, survey, "نه اصلا",0)),
                                InlineKeyboardButton.WithCallbackData("خیلی کم",SerializeSurveyButtonData(message, consideredUserId, survey, "خیلی کم",25)),
                                InlineKeyboardButton.WithCallbackData("متوسط",SerializeSurveyButtonData(message, consideredUserId, survey, "متوسط",50)),
                                InlineKeyboardButton.WithCallbackData("آره",SerializeSurveyButtonData(message, consideredUserId, survey, "آره",75)),
                                InlineKeyboardButton.WithCallbackData("آره خیلی",SerializeSurveyButtonData(message, consideredUserId, survey, "آره خیلی",100))
                            },
                            new[]
                            {
                            InlineKeyboardButton.WithCallbackData("اینو بیخیال","SkipSurvey"),
                            InlineKeyboardButton.WithCallbackData("بازگشت","MainMenu")
                            }
                        });
        }

        internal static void SkipVote(CallbackQueryEventArgs e)
        {
            List<string> data = e.CallbackQuery.Data.Split("_").ToList();
            Application.BotClient.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId);
            SendNextSurvey(e, int.Parse(data[3]), int.Parse(data[2]));
        }


        private static string SerializeSurveyButtonData(Message message, int consideredUserId, DataAccess.Models.Survey survey, string surveyName, int point)
        {
            return "Survey_" + message.From.Id + "_" + consideredUserId + "_" + survey.Id + "_" + surveyName + "_" + point;
        }

        public static bool CheckIsSurvey(MessageEventArgs e, out int userId)
        {
            userId = 0;
            return e.Message.Text.StartsWith("/start") &&
                            e.Message.Text.Split(" ")[0] == "/start" &&
                            !e.Message.Text.Split(" ")[1].IsNullOrEmptyOrWhitespace() &&
                            e.Message.Text.Split(" ")[1].Split("-")[0] == "PL" &&
                            !e.Message.Text.Split(" ")[1].Split("-")[1].IsNullOrEmptyOrWhitespace() &&
                            Int32.TryParse(e.Message.Text.Split(" ")[1].Split("-")[1], out userId);
        }



        public static async Task CreateAndSendSurveyInlineKeyboard(MessageEventArgs e, int consideredUserId)
        {
            if (Application.BotClient == null || e == null)
                return;
            DataAccess.Models.User user = Business.GetUser(consideredUserId);
            if (null == user)
            {
                await Application.BotClient.SendTextMessageAsync(
                    e.Message.Chat.Id,
                    "کاربر یافت نشد"
                ).ConfigureAwait(true);
                return;
            }
            await Application.BotClient.SendTextMessageAsync(
                e.Message.Chat.Id,
                string.Format("شما الان در حال اظهار نظر در مورد ویژگی های {0} هستید.میتونید مطمئن باشید این رای گیری همونطور که برای شما مخفیه برای {0} هم مخفی خواهد بود.", user.FirstName)
                ).ConfigureAwait(true);

            SendSurvey(Consts.Surveys.First(), e.Message, consideredUserId);

        }

        private static void SendSurvey(DataAccess.Models.Survey survey, Message message, int consideredUserId)
        {
            if (survey == null)
                return;
            Application.BotClient.SendTextMessageAsync(
                message.Chat.Id,
                survey.PersianTitle,
                replyMarkup: Statics.GetSurveyButtons(message, consideredUserId, survey)
            ).ConfigureAwait(false);
            SendSurveyFooterMessage(message);
        }

        private static void SendSurveyFooterMessage(Message message)
        {
            Application.BotClient.SendTextMessageAsync(
                message.Chat.Id,
                " ",
                replyMarkup: new InlineKeyboardMarkup(new[]
                {
                    InlineKeyboardButton.WithCallbackData("بازگشت", "MainMenu")
                })
            ).ConfigureAwait(false);
        }

        public static void GetPrivateLink(CallbackQueryEventArgs e)
        {
            if (Application.BotClient == null || e == null)
                return;
            Application.BotClient.SendTextMessageAsync(
                e.CallbackQuery.Message.Chat.Id,
                Messages.BotURL + "PL-" + e.CallbackQuery.From.Id
            ).ConfigureAwait(false);
        }

        public static void StartClient(Message message)
        {
            if (Application.BotClient == null || message == null)
                return;
            Telegram.Bot.Types.User user = message.From;
            if (!Business.CreateUser(new DataAccess.Models.User
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                lastName = user.LastName
            }))
                return;

            GetMainMenu(message);

        }

        public static void GetMainMenu(Message message)
        {
            Application.BotClient.SendTextMessageAsync(
                message.Chat.Id,
                Messages.StartClient,
                replyMarkup: Statics.GetMainMenu()
            ).ConfigureAwait(false);
        }

        public static void GetReport(CallbackQueryEventArgs e)
        {
            List<SurveySummary> result = Business.GetUserSurveySummery(e.CallbackQuery.From.Id);
            if(result == null || result.Count == 0)
            {
                Application.BotClient.SendTextMessageAsync(
                e.CallbackQuery.Message.Chat.Id,
                string.Format("تا الان هیچ نظری در مورد شما داده نشده ..."),
                replyMarkup: new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("بازگشت","MainMenu")
                    }
                })
                ).ConfigureAwait(true);
                return;
            }
            string message = " نظرات ثبت شده برای شماایناس :" + System.Environment.NewLine;
            foreach (SurveySummary summery in result)
            {
                message += summery.SurveyPersianName + " = " + summery.Point + "% با " + summery.SurveyCount + " رای" + System.Environment.NewLine;
            }
            Application.BotClient.SendTextMessageAsync(
                e.CallbackQuery.Message.Chat.Id,
                string.Format(message),
                replyMarkup: new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("بازگشت","MainMenu")
                    }
                })
                ).ConfigureAwait(true);
                return;
        }

        public static void SendNewSurvey(MessageEventArgs e, in int userId)
        {
            Telegram.Bot.Types.User user = e.Message.From;
            CreateUser createUser = new CreateUser(new DataAccess.Models.User
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                lastName = user.LastName
            });
            createUser.Run();
            CreateAndSendSurveyInlineKeyboard(e, userId);
        }

        public static void SetVote(CallbackQueryEventArgs e)
        {
            List<string> data = e.CallbackQuery.Data.Split("_").ToList();
            UserSurvey userSurvey = new UserSurvey
            {
                VoterUserId = e.CallbackQuery.From.Id,
                ConsideredUserId = int.Parse(data[2]),
                SurveyId = int.Parse(data[3]),
                Point = int.Parse(data[5]),
                SurveyDate = DateTime.Now
            };

            if (!Business.InsertUserSurvey(userSurvey)) return;
            Application.BotClient.DeleteMessageAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId);
            SendNextSurvey(e, userSurvey.SurveyId, userSurvey.ConsideredUserId);
        }

        private static async Task SendNextSurvey(CallbackQueryEventArgs e, int surveyId, int consideredUserId)
        {
            if (Consts.Surveys.Last().Id == surveyId)
            {
                await SurveyFinished(e.CallbackQuery.Message);
                return;
            }
            SendSurvey(GetNextSurvey(surveyId), e.CallbackQuery.Message, consideredUserId);
        }

        private static async Task SurveyFinished(Message message)
        {
            await Application.BotClient.SendTextMessageAsync(
                message.Chat.Id,
                "نظرسنجی به پابان رسید.ازتون ممنونیم ",
                replyMarkup: new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("بازگشت","MainMenu")
                    }
                })
            ).ConfigureAwait(true);
        }

        private static DataAccess.Models.Survey GetNextSurvey(int surveyId)
        {
            for (int i = 0; i < Consts.Surveys.Count; i++)
            {
                if (Consts.Surveys[i].Id != surveyId)
                {
                    continue;
                }
                return i == Consts.Surveys.Count - 1 ? null : Consts.Surveys[i + 1];
            }
            return null;
        }
    }

}
