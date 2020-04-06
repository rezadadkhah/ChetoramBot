using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace ChetoramBot.Helpers
{
    public static class Extentions
    {
        public static int GetMyBotId(TelegramBotClient botClient)
        {
            return botClient.GetMeAsync().Result.Id;
        }

        public static Telegram.Bot.Types.User GetMe(TelegramBotClient botClient)
        {
            return botClient.GetMeAsync().Result;
        }

        public static bool IsNullOrEmptyOrWhiteSpace(string text)
        {
            return text == null || string.IsNullOrWhiteSpace(text);
        }

        public static bool IsPrivateLink(this string text)
        {
            return text.StartsWith("/start") &&
                text.Split(" ")[0] == "/start" &&
                !string.IsNullOrEmpty(text.Split(" ")[1]) &&
                text.Split(" ")[1].Split("-")[0] == "PL" &&
                !string.IsNullOrEmpty(text.Split(" ")[1].Split("-")[1]) &&
                int.TryParse(text.Split(" ")[1].Split("-")[1], out int userId);
        }
    }
}
