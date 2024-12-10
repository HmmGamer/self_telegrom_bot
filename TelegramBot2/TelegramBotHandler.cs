using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot2.Handler
{
    public class TelegramBotHandler
    {
        public static TelegramBotClient _botClient
            = new TelegramBotClient("7590120796:AAEd3CPsO4CBjrgDiXZSXDrEx_jzT2yszKM");
        public static long _chatId;

        public TelegramBotHandler()
        {
            StartReceivingMessagesAsync();
        }

        private void StartReceivingMessagesAsync()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            _botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                cancellationToken: cancellationTokenSource.Token
            );
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, System.Threading.CancellationToken cancellationToken)
        {
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message && update.Message?.Text != null)
            {
                _chatId = update.Message.Chat.Id;
                string userName = update.Message.From?.Username ?? "Unknown";

                // Get the bot's username
                var botUsername = (await botClient.GetMeAsync()).Username;

                // Check if the message is a command and possibly includes the bot's username
                string messageText = update.Message.Text.Trim();
                if (/*messageText.StartsWith("/") &&*/
                    (messageText.EndsWith($"@{botUsername}") || !messageText.Contains('@')))
                {
                    // Remove the bot's @username if present
                    messageText = messageText.Replace($"@{botUsername}", "").Trim();

                    // Pass the cleaned message text to your message handling logic
                    MessageChecker messageChecker = new MessageChecker();
                    await messageChecker._CheckMsgFunction(messageText, userName);
                }
            }
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, System.Threading.CancellationToken cancellationToken)
        {
            Console.WriteLine($"Error occurred: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}
