using Business.Business.User;
using Business.Common.Extentions;
using System;
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
            
            botClient.OnUpdate += OnUpdate;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        private static async void OnUpdate(object sender, UpdateEventArgs e)
        {
            var handler = e.Update.Type switch
            {
                UpdateType.Message => ProcessMessage(e),
                _ => UnknownUpdateHandlerAsync(e.Update)
            };

            try
            {
                await handler;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async Task UnknownUpdateHandlerAsync(Update update)
        {
            Console.WriteLine($"Unknown update type: {update.Type}");
        }

        private static async Task ProcessMessage(UpdateEventArgs e)
        {
            var handler = e.Update.Message.Type switch
            {
                MessageType.Text => ProcessText(e),
                _ => UnknownUpdateHandlerAsync(e.Update)
            };
            await handler;
            
        }

        private static async Task ProcessText(UpdateEventArgs e)
        {
            if(e.Update.Message.Text == "/start")
            {
                StartClient(e);
                return;
            }
            if(e.Update.Message.Text == "لینک نظردهی ناشناس من")
            {
                GetPrivateLink(e);
                return;
            }    
            if(e.Update.Message.Text.StartsWith("/start") && 
                e.Update.Message.Text.Split(" ")[0] == "/start" &&
                !e.Update.Message.Text.Split(" ")[1].IsNullOrEmptyOrWhitespace() &&
                e.Update.Message.Text.Split(" ")[1].Split("-")[0] == "PL" &&
                !e.Update.Message.Text.Split(" ")[1].Split("-")[0].IsNullOrEmptyOrWhitespace() &&
                int.TryParse(e.Update.Message.Text.Split(" ")[1].Split("-")[0],out int userId))
            {
                GetNewServey(userId);
                return;
            }
        }

        private static void GetNewServey(int userId)
        {
            throw new NotImplementedException();
        }

        private static async Task GetPrivateLink(UpdateEventArgs e)
        {
            await botClient.SendTextMessageAsync(
                            chatId: e.Update.Message.Chat.Id,
                            text: Messages.BotURL + "PL-" + e.Update.Message.From.Id
                        ).ConfigureAwait(false);
        }

        private static async Task StartClient(UpdateEventArgs e)
        {
            User user = e.Update.Message.From;
            CreateUser CreateUser = new CreateUser(new DataAccess.Models.User
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                lastName = user.LastName
            });
            CreateUser.Run();
            await botClient.SendTextMessageAsync(
                            chatId: e.Update.Message.Chat.Id,
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
                            new KeyboardButton("لینک نظردهی ناشناس من")
                        },
                        new[]
                        {
                            KeyboardButton.WithRequestContact("نظر بقیه راجع بهم چی بوده؟")
                        }
                    }
            );
        }
        


    }
}
