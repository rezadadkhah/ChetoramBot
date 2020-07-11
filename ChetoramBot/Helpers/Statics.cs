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
                InlineKeyboardButton.WithCallbackData(StaticMessages.MyPrivateLink, "MyPL")
            };
            List<InlineKeyboardButton> secondRow = new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(StaticMessages.MyReport, "MyReport")
            };
            List<InlineKeyboardButton> thirdRow = new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(StaticMessages.MainMenu, "MainMenu")
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
                                InlineKeyboardButton.WithCallbackData(StaticMessages.Never,SerializeSurveyButtonData(message, consideredUserId, survey, StaticMessages.Never,0)),
                                InlineKeyboardButton.WithCallbackData(StaticMessages.Little,SerializeSurveyButtonData(message, consideredUserId, survey, StaticMessages.Little,25)),
                                InlineKeyboardButton.WithCallbackData(StaticMessages.Medium,SerializeSurveyButtonData(message, consideredUserId, survey, StaticMessages.Medium,50)),
                                InlineKeyboardButton.WithCallbackData(StaticMessages.AlmostTooMuch,SerializeSurveyButtonData(message, consideredUserId, survey, StaticMessages.AlmostTooMuch,75)),
                                InlineKeyboardButton.WithCallbackData(StaticMessages.Certainly,SerializeSurveyButtonData(message, consideredUserId, survey, StaticMessages.Certainly,100))
                            },
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData(StaticMessages.SkipThis,"SkipSurvey"),
                                InlineKeyboardButton.WithCallbackData(StaticMessages.Return,"MainMenu")
                            }
                        });
        }

        internal static void SkipVote(CallbackQuery callbackQuery)
        {
            List<string> data = callbackQuery.Data.Split("_").ToList();
            Application.BotClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
            SendNextSurvey(callbackQuery, int.Parse(data[3]), int.Parse(data[2]));
        }


        private static string SerializeSurveyButtonData(Message message, int consideredUserId, DataAccess.Models.Survey survey, string surveyName, int point)
        {
            return "Survey_" + message.From.Id + "_" + consideredUserId + "_" + survey.Id + "_" + surveyName + "_" + point;
        }

        public static bool CheckIsSurvey(Message message, int userId) =>
                            message.Text.StartsWith("/start") &&
                            message.Text.Split(" ")[0] == "/start" &&
                            !message.Text.Split(" ")[1].IsNullOrEmptyOrWhitespace() &&
                            message.Text.Split(" ")[1].Split("-")[0] == "PL" &&
                            !message.Text.Split(" ")[1].Split("-")[1].IsNullOrEmptyOrWhitespace() &&
                            Int32.TryParse(message.Text.Split(" ")[1].Split("-")[1], out userId);



        public static void CreateAndSendSurveyInlineKeyboard(Message message, int consideredUserId)
        {
            if (Application.BotClient == null || message == null)
                return;
            DataAccess.Models.User user = Business.GetUser(consideredUserId);
            if (null == user)
            {
                Application.BotClient.SendTextMessageAsync(
                    message.Chat.Id,
                    "کاربر یافت نشد"
                );
                return;
            }
            Application.BotClient.SendTextMessageAsync(
                message.Chat.Id,
                string.Format("شما الان در حال اظهار نظر در مورد ویژگی های {0} هستید.میتونید مطمئن باشید این رای گیری همونطور که برای شما مخفیه برای {0} هم مخفی خواهد بود.", user.FirstName)
                );

            SendSurvey(Consts.Surveys.First(), message, consideredUserId);

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

        public static void GetPrivateLink(CallbackQuery callbackQuery)
        {
            if (Application.BotClient == null || callbackQuery == null)
                return;
            Application.BotClient.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                Messages.BotURL + "PL-" + callbackQuery.From.Id
            );
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
            );
        }
                      
        public static void GetReport(CallbackQuery callbackQuery)
        {
            List<SurveySummary> result = Business.GetUserSurveySummery(callbackQuery.From.Id);
            if (result == null || result.Count == 0)
            {
                Application.BotClient.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                string.Format("تا الان هیچ نظری در مورد شما داده نشده ..."),
                replyMarkup: new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("بازگشت","MainMenu")
                    }
                })
                );
                return;
            }
            string message = " نظرات ثبت شده برای شماایناس :" + System.Environment.NewLine;
            foreach (SurveySummary summery in result)
            {
                message += summery.SurveyPersianName + " = " + summery.Point + "% با " + summery.SurveyCount + " رای" + System.Environment.NewLine;
            }
            Application.BotClient.SendTextMessageAsync(
               callbackQuery.Message.Chat.Id,
                string.Format(message),
                replyMarkup: new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("بازگشت","MainMenu")
                    }
                })
                );
            return;
        }
                      
        public static void SendNewSurvey(Message message,int userId)
        {
            CheckNull(message);
            Telegram.Bot.Types.User user = message.From;
            CreateUser createUser = new CreateUser(CreateUserModel(user));
            createUser.Run();
            CreateAndSendSurveyInlineKeyboard(message, userId);
        }

        private static DataAccess.Models.User CreateUserModel(Telegram.Bot.Types.User user)
        {
            return new DataAccess.Models.User
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                lastName = user.LastName
            };
        }

        private static void CheckNull(object obj)
        {
            if(obj == null)
                throw new NullReferenceException();
        }

        public static void SetVote(CallbackQuery callbackQuery)
        {
            List<string> data = callbackQuery.Data.Split("_").ToList();
            UserSurvey userSurvey = new UserSurvey
            {
                VoterUserId = callbackQuery.From.Id,
                ConsideredUserId = int.Parse(data[2]),
                SurveyId = int.Parse(data[3]),
                Point = int.Parse(data[5]),
                SurveyDate = DateTime.Now
            };

            if (!Business.InsertUserSurvey(userSurvey)) return;
            Application.BotClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId);
            SendNextSurvey(callbackQuery, userSurvey.SurveyId, userSurvey.ConsideredUserId);
        }

        private static void SendNextSurvey(CallbackQuery callbackQuery, int surveyId, int consideredUserId)
        {
            if (Consts.Surveys.Last().Id == surveyId)
            {
                SurveyFinished(callbackQuery.Message);
                return;
            }
            SendSurvey(GetNextSurvey(surveyId), callbackQuery.Message, consideredUserId);
        }

        private static void SurveyFinished(Message message)
        {
            Application.BotClient.SendTextMessageAsync(
                message.Chat.Id,
                "نظرسنجی به پابان رسید.ازتون ممنونیم ",
                replyMarkup: new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("بازگشت","MainMenu")
                    }
                })
            );
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
