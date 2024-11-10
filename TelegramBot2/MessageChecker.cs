using System;
using System.Text.RegularExpressions;
using TelegramBot2.Data;

namespace TelegramBot2
{
    public class MessageChecker
    {
        public async Task _CheckMsgFunction(string iMsg, string userName)
        {
            if (iMsg.Length < 5) return;
            if (iMsg.StartsWith("#sell"))
            {
                _CheckForNewOrder(iMsg, _AllOrderTypes.sell, userName);
            }
            else if (iMsg.StartsWith("#buy"))
            {
                _CheckForNewOrder(iMsg, _AllOrderTypes.buy, userName);
            }
            else if (iMsg.StartsWith("/"))
            {
                await _CheckForCommand(iMsg);
            }
        }

        public void _CheckForNewOrder(string iNewOrder, _AllOrderTypes iType, string userName)
        {
            bool foodAdded = false;

            foreach (var foodType in _FoodType._AllFoodTypes)
            {
                string foodNamesPattern = $"{foodType._foodName}|{string.Join("|", foodType._alternativeNames)}";
                string pattern = $@"\b({foodNamesPattern})\b\s+(\d+(\.\d+)?)";
                Match match = Regex.Match(iNewOrder, pattern, RegexOptions.IgnoreCase);

                if (match.Success &&
                    float.TryParse(match.Groups[2].Value, out float foodPrice) &&
                    foodPrice >= foodType._minPrice &&
                    foodPrice <= foodType._maxPrice)
                {
                    LogDataBase._AddLog(foodType, iType, foodPrice, userName);
                    Console.WriteLine($"The food {foodType} was added by {userName}.");
                    foodAdded = true;
                    break;
                }
            }
            if (!foodAdded)
            {
                Console.WriteLine("No matching food found in database.");
            }
            DataStorage.SaveLogsToCsv();
        }
        public async Task _CheckForCommand(string iCmd)
        {
            string command = iCmd.Substring(1).Trim();
            string[] commandParts = command.Split(' ', 2);
            string baseCommand = commandParts[0].ToLower();
            string foodName = commandParts.Length > 1 ? commandParts[1].Trim() : null;

            string result;

            if (baseCommand.Equals("show_buy_logs", StringComparison.OrdinalIgnoreCase))
            {
                result = foodName != null
                    ? LogDataBase._GetSpecificBuyLog(foodName)
                    : LogDataBase._ShowBuyLogs();

                Console.WriteLine(result);
                await MessageSender.Instance._SendMessageAsync(result);
            }
            else if (baseCommand.Equals("show_sell_logs", StringComparison.OrdinalIgnoreCase))
            {
                result = foodName != null
                    ? LogDataBase._GetSpecificSellLog(foodName)
                    : LogDataBase._ShowSellLogs();

                Console.WriteLine(result);
                await MessageSender.Instance._SendMessageAsync(result);
            }
            else if (baseCommand.Equals("show_average", StringComparison.OrdinalIgnoreCase) && foodName != null)
            {
                result = LogDataBase._ShowFoodAverage(foodName);
                Console.WriteLine(result);
                await MessageSender.Instance._SendMessageAsync(result);
            }
            else
            {
                Console.WriteLine("Unknown command.");
            }
        }

    }
}