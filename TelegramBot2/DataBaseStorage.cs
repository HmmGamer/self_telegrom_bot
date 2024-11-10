using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace TelegramBot2.Data
{
    public static class DataStorage
    {
        const int _USER_NAME_LENGTH = 12;

        // Method to save logs to CSV files
        public static void SaveLogsToCsv()
        {
            // Save sell logs
            SaveToCsv(LogDataBase._sellDataBase, "sell_logs.csv");

            // Save buy logs
            SaveToCsv(LogDataBase._buyDataBase, "buy_logs.csv");
        }

        // Method to save a list of logs to a CSV file
        private static void SaveToCsv(List<FoodLogStructure> logData, string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    // Write CSV header
                    writer.WriteLine("FoodType,FoodPrice,UserName,OrderTime");

                    // Write the log entries to the file
                    foreach (var log in logData)
                    {
                        string userName = log._userName.Length > _USER_NAME_LENGTH ? log._userName.Substring(0, _USER_NAME_LENGTH) : log._userName;
                        writer.WriteLine($"{log._foodType._foodName},{log._foodPrice},{userName},{log._orderTime}");
                    }
                }

                Console.WriteLine($"Logs have been successfully saved to {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving to CSV: {ex.Message}");
            }
        }

        // Method to load logs from CSV files
        public static void LoadLogsFromCsv()
        {
            // Load sell logs from CSV
            LoadFromCsv("sell_logs.csv", _AllOrderTypes.sell);

            // Load buy logs from CSV
            LoadFromCsv("buy_logs.csv", _AllOrderTypes.buy);
        }

        // Method to load logs from a specific CSV file
        private static void LoadFromCsv(string fileName, _AllOrderTypes orderType)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    Console.WriteLine($"File {fileName} not found.");
                    return;
                }

                using (StreamReader reader = new StreamReader(fileName))
                {
                    // Skip header
                    reader.ReadLine();

                    // Read each line from the CSV and parse it
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        if (values.Length != 4)
                        {
                            Console.WriteLine($"Invalid line format: {line}");
                            continue;
                        }

                        // Parse the values from the CSV line
                        string foodName = values[0].Trim();
                        float foodPrice = float.Parse(values[1].Trim(), CultureInfo.InvariantCulture);
                        string userName = values[2].Trim();
                        string orderTime = values[3].Trim();

                        // Find the corresponding food type
                        _FoodType? foodType = _FoodType._AllFoodTypes.Find(f =>
                            f._foodName.Equals(foodName, StringComparison.OrdinalIgnoreCase) ||
                            Array.Exists(f._alternativeNames, alt => alt.Equals(foodName, StringComparison.OrdinalIgnoreCase)));

                        if (foodType != null)
                        {
                            FoodLogStructure logEntry = new FoodLogStructure
                            {
                                _foodType = foodType,
                                _orderType = orderType,
                                _foodPrice = foodPrice,
                                _userName = userName,
                                _orderTime = orderTime
                            };

                            // Add the log entry to the appropriate database
                            if (orderType == _AllOrderTypes.sell)
                            {
                                LogDataBase._sellDataBase.Add(logEntry);
                            }
                            else
                            {
                                LogDataBase._buyDataBase.Add(logEntry);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Food type '{foodName}' not found in the predefined food types.");
                        }
                    }
                }

                Console.WriteLine($"Logs have been successfully loaded from {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading from CSV: {ex.Message}");
            }
        }
    }
}
