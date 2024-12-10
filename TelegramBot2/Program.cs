using System;
using System.Net.Http;
using System.Threading.Tasks;
using TelegramBot2.Data;
using TelegramBot2.Handler;

namespace TelegramBot2
{
    public class Program
    {
        public static async Task Main()
        {
            string botToken = "7590120796:AAEd3CPsO4CBjrgDiXZSXDrEx_jzT2yszKM";
            _DataSet.InitializeFoodTypes();

            MessageChecker messenger = new MessageChecker();
            TelegramBotHandler botHandler = new TelegramBotHandler();

            DataStorage.LoadLogsFromCsv();

            // Check the connection to Telegram API
            bool isConnected = await CheckTelegramApiConnection(botToken);
            if (isConnected)
            {
                Console.WriteLine("Successfully connected to the Telegram API.");
            }
            else
            {
                Console.WriteLine("Failed to connect to the Telegram API.");
                return; // Exit if not connected
            }

            await CommandRegistry._RegisterCommandsAsync(botToken);
            Console.WriteLine("Bot is running...");
            while (true)
            {
                Task.Delay(1000).Wait();
            }
        }

        // Method to check connection to the Telegram API
        private static async Task<bool> CheckTelegramApiConnection(string botToken)
        {
            string url = $"https://api.telegram.org/bot{botToken}/getMe";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        return true; // Successfully connected
                    }
                    else
                    {
                        Console.WriteLine($"Failed to connect to the Telegram API. Status code: {response.StatusCode}");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error connecting to the Telegram API: {ex.Message}");
                    return false;
                }
            }
        }
    }
}
