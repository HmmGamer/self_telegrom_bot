using System.Text.Json;

namespace TelegramBot2
{
    public static class CommandRegistry
    {
        private static readonly Dictionary<string, string> _commands = new Dictionary<string, string>
        {
            { "/show_buy_logs", "Displays the logs of all buy transactions." },
            { "/show_sell_logs", "Displays the logs of all sell transactions." },
            { "/show_average", "Shows an average of a food prices"}
        };

        public static async Task _RegisterCommandsAsync(string botToken)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"https://api.telegram.org/bot{botToken}/setMyCommands";
                var commandsList = new List<object>();

                foreach (var command in _commands)
                {
                    commandsList.Add(new { command = command.Key.TrimStart('/'), description = command.Value });
                }

                var payload = new { commands = commandsList };
                var jsonContent = new StringContent(JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(url, jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Commands registered successfully.");
                }
                else
                {
                    Console.WriteLine($"Failed to register commands. Status: {response.StatusCode}");
                }
            }
        }

        public static Dictionary<string, string> _GetCommands()
        {
            return _commands;
        }
    }
}
