using Result;

namespace TelegramService.Interfaces
{
    public interface ITelegramService
    {
        /// <summary>
        /// Асинхронный метод для отправки сообщения в телеграм
        /// </summary>
        public Task<BaseResult<string>> SendMessageAsync(string message);
    }
}
