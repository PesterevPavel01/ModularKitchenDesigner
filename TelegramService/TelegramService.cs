using Resources;
using Resources.Enum;
using Result;
using Telegram.Bot;
using TelegramService.Interfaces;

namespace TelegramService
{
    public class TelegramService : ITelegramService
    {
        private static string _token { get; set; } = "7562655253:AAEKJQnd1YXSXpEXtQJ0NvJSo3C1B-GEN8E";
        private static TelegramBotClient TelegramBotClient = null!;
        private static readonly string channelId = "-1002463757906";

        public TelegramService()
        {
            TelegramBotClient = new(_token);
        }

        public async Task<BaseResult<string>> SendMessageAsync(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                try
                {
                    await TelegramBotClient.SendTextMessageAsync(channelId, message);
                    return new()
                    {
                        Data = "Уведомление отправлено"
                    };
                }
                catch (Exception ex)
                {
                    return new()
                    {
                        ObjectName = "TelegramBotClient",
                        ErrorMessage = ex.Message,
                    };
                }
            }
            else
            {
                return new()
                {
                    ErrorMessage = ErrorMessages.IncorrectInputObject,
                    ErrorCode = (int)ErrorCodes.IncorrectInputObject
                };
            }
        }

        public BaseResult<string> SendMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                try
                {
                    TelegramBotClient.SendTextMessageAsync(channelId, message).GetAwaiter().GetResult();
                    return new()
                    {
                        Data = "Уведомление отправлено"
                    };
                }
                catch (Exception ex)
                {
                    return new()
                    {
                        ObjectName = "TelegramBotClient",
                        ErrorMessage = ex.Message,
                    };
                }
            }
            else
            {
                return new()
                {
                    ErrorMessage = ErrorMessages.IncorrectInputObject,
                    ErrorCode = (int)ErrorCodes.IncorrectInputObject
                };
            }
        }
    }
}
