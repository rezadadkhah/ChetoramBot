using System;
using Business.Business.User;
using Business.Common.Extentions;
using DataAccess.Models;
using System.Collections.Generic;
using ChetoramBot.ViewModels;
using Newtonsoft.Json;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

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
            buttons.Add(firstRow);
            buttons.Add(secondRow);
            return new InlineKeyboardMarkup(buttons);
        }

        public static IReplyMarkup GetSurveyButtons(MessageEventArgs e, int userId, Survey survey)
        {
            if (e == null || survey == null)
                return null;
            return new InlineKeyboardMarkup(new[]
                        {
                            InlineKeyboardButton.WithCallbackData("نه اصلا",JsonConvert.SerializeObject(new SurveyVm()
                            {
                                VId = e.Message.From.Id,
                                CId = userId,
                                SId = survey.Id,
                                Sn = "نه اصلا",
                                P = 0
                            })),
                            InlineKeyboardButton.WithCallbackData("خیلی کم",JsonConvert.SerializeObject(new SurveyVm()
                            {
                                VId = e.Message.From.Id,
                                CId = userId,
                                SId = survey.Id,
                                Sn = "خیلی کم",
                                P = 25
                            })),
                            InlineKeyboardButton.WithCallbackData("متوسط",JsonConvert.SerializeObject(new SurveyVm()
                            {
                                VId = e.Message.From.Id,
                                CId = userId,
                                SId = survey.Id,
                                Sn = "متوسط",
                                P = 50
                            })),
                            InlineKeyboardButton.WithCallbackData("آره",JsonConvert.SerializeObject(new SurveyVm()
                            {
                                VId = e.Message.From.Id,
                                CId = userId,
                                SId = survey.Id,
                                Sn = "آره",
                                P = 75
                            })),
                            InlineKeyboardButton.WithCallbackData("آره خیلی",JsonConvert.SerializeObject(new SurveyVm()
                            {
                                VId = e.Message.From.Id,
                                CId = userId,
                                SId = survey.Id,
                                Sn = "آره خیلی",
                                P = 100
                            }))
                        });
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

        public static void InsertUserSurvey(UserSurvey userSurvey)
        {
            InsertUserSurvey insertUserSurvey = new InsertUserSurvey(userSurvey);
            insertUserSurvey.Run();
        }

        public static void CreateAndSendSurveyInlineKeyboard(MessageEventArgs e, int userId, IEnumerable<Survey> surveys)
        {
            if (surveys == null || Application.BotClient == null || e == null)
                return;
            foreach (Survey survey in surveys)
            {
                Application.BotClient.SendTextMessageAsync(
                    e.Message.Chat.Id,
                    survey.PersianTitle,
                    replyMarkup: Statics.GetSurveyButtons(e, userId, survey)
                ).ConfigureAwait(false);

            }
        }

        private static void GetPrivateLink(CallbackQueryEventArgs e)
        {
            if (Application.BotClient == null || e == null)
                return;
            Application.BotClient.SendTextMessageAsync(
                e.CallbackQuery.Message.Chat.Id,
                Messages.BotURL + "PL-" + e.CallbackQuery.Message.From.Id
            ).ConfigureAwait(false);
        }

        public static void StartClient(MessageEventArgs e)
        {
            if (Application.BotClient == null || e == null)
                return;
            Telegram.Bot.Types.User user = e.Message.From;
            CreateUser createUser = new CreateUser(new User
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                lastName = user.LastName
            });
            createUser.Run();
            Application.BotClient.SendTextMessageAsync(
                e.Message.Chat.Id,
                Messages.StartClient,
                replyMarkup: Statics.GetMainMenu()
            ).ConfigureAwait(false);
        }

        public static bool IsSurveyVm(SurveyVm surveyVm)
        {
            return surveyVm != null &&
                   surveyVm.CId.IsPositive() &&
                   surveyVm.VId.IsPositive() &&
                   surveyVm.SId.IsPositive() &&
                   surveyVm.P.IsPositive() &&
                   !surveyVm.Sn.IsNullOrEmptyOrWhitespace();
        }


        public static void ProcessCallbackQuery(CallbackQueryEventArgs e)
        {
            switch (e.CallbackQuery.Data)
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
                default:
                {
                    SurveyVm surveyVm = JsonConvert.DeserializeObject<SurveyVm>(e.CallbackQuery.Data);
                    if (Statics.IsSurveyVm(surveyVm))
                    {
                        return;
                    }
                    UserSurvey userSurvey = new UserSurvey()
                    {
                        VoterUserId = surveyVm.VId,
                        ConsideredUserId = surveyVm.CId,
                        SurveyId = surveyVm.SId,
                        Point = surveyVm.P,
                    };
                    InsertUserSurvey(userSurvey);
                    Application.BotClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id);
                    Application.BotClient.EditMessageTextAsync(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.Message.MessageId, e.CallbackQuery.Message.Text + " => " + surveyVm.Sn);
                    break;
                }
            }
        }

        private static void GetReport(CallbackQueryEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

}
