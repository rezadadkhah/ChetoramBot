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
    }
}
