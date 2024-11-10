using Telegram.Bot;
using System;
using TelegramBot2.Handler;

public class MessageSender
{
    private static readonly Lazy<MessageSender> _instance = new Lazy<MessageSender>(() => new MessageSender());

    public static MessageSender Instance => _instance.Value;

    public async Task _SendMessageAsync(string messageText)
    {
        if (string.IsNullOrWhiteSpace(messageText)) return;

        await TelegramBotHandler._botClient.SendMessage(
            chatId : TelegramBotHandler._chatId,
            text: messageText,
            parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
            disableNotification: false
        );
    }
}
