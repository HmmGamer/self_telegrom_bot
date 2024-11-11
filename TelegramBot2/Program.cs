using System;
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
            //await messenger._CheckMsgFunction("/show_sell_logs", "taha");
            

            //await messenger._CheckMsgFunction("#sell kebab 20", "taha");
            //await messenger._CheckMsgFunction("#sell joje 20", "mmd");
            

            //await messenger._CheckMsgFunction("#sell kebab 20", "dna");
            
            
            await CommandRegistry._RegisterCommandsAsync(botToken);
            Console.WriteLine("Bot is running...");
            while (true)
            {
                Task.Delay(1000).Wait();
            }
            
            //Console.ReadLine();
        }
    }
}