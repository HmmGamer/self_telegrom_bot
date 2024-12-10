namespace TelegramBot2.Data
{
    public static class LogDataBase
    {
        const int _USER_NAME_LENGTH = 12;
        public static List<FoodLogStructure> _sellDataBase;
        public static List<FoodLogStructure> _buyDataBase;

        static LogDataBase()
        {
            _sellDataBase = new List<FoodLogStructure>();
            _buyDataBase = new List<FoodLogStructure>();
        }

        public static void _AddLog(_FoodType iFoodType, _AllOrderTypes iOrderType, float iFoodPrice, string iUserName)
        {
            FoodLogStructure logEntry = new FoodLogStructure
            {
                _foodType = iFoodType,
                _orderType = iOrderType,
                _foodPrice = iFoodPrice,
                _userName = iUserName,
                _orderTime = DateTime.Now.ToString("HH:mm ddd")  // Store in the correct format
            };

            List<FoodLogStructure> targetDatabase = (iOrderType == _AllOrderTypes.sell) ? _sellDataBase : _buyDataBase;

            for (int i = 0; i < targetDatabase.Count; i++)
            {
                if (targetDatabase[i]._userName == iUserName)
                {
                    targetDatabase[i] = logEntry;

                    // Sorting with TryParseExact to prevent errors due to incorrect formats
                    targetDatabase.Sort((x, y) =>
                    {
                        DateTime dateX, dateY;
                        bool parsedX = DateTime.TryParseExact(x._orderTime, "HH:mm ddd", null, System.Globalization.DateTimeStyles.None, out dateX);
                        bool parsedY = DateTime.TryParseExact(y._orderTime, "HH:mm ddd", null, System.Globalization.DateTimeStyles.None, out dateY);

                        if (!parsedX || !parsedY)
                        {
                            // Handle the case where the date is invalid (e.g., set a default value or handle it based on your requirements)
                            return 0;  // Do not change order if there's an invalid date
                        }

                        return dateY.CompareTo(dateX);  // Sort in descending order
                    });

                    return;
                }
            }

            targetDatabase.Insert(0, logEntry);
        }
        public static string _ShowSellLogs()
        {
            List<string> logEntries = new List<string>();
            foreach (var log in _sellDataBase)
            {
                string userName = log._userName?.Length > _USER_NAME_LENGTH ? log._userName.Substring(0, _USER_NAME_LENGTH) : log._userName ?? "";
                logEntries.Add($"{userName} => {log._foodType._foodName}: {log._foodPrice}, {log._orderTime}");
            }
            return string.Join("\n", logEntries);
        }

        public static string _ShowBuyLogs()
        {
            List<string> logEntries = new List<string>();
            foreach (var log in _buyDataBase)
            {
                string userName = log._userName?.Length > _USER_NAME_LENGTH ? log._userName.Substring(0, _USER_NAME_LENGTH) : log._userName ?? "";
                logEntries.Add($"{userName} => {log._foodType._foodName}: {log._foodPrice}, {log._orderTime}");
            }
            return string.Join("\n", logEntries);
        }

        public static string _GetSpecificBuyLog(string iFoodName)
        {
            List<string> logEntries = new List<string>();
            foreach (var log in _buyDataBase)
            {
                if (log._foodType._foodName.Equals(iFoodName, StringComparison.OrdinalIgnoreCase) ||
                    Array.Exists(log._foodType._alternativeNames, alt => alt.Equals(iFoodName, StringComparison.OrdinalIgnoreCase)))
                {
                    string userName = log._userName?.Length > _USER_NAME_LENGTH ? log._userName.Substring(0, _USER_NAME_LENGTH) : log._userName ?? "";
                    logEntries.Add($"{userName} => {log._foodType._foodName}: {log._foodPrice}, {log._orderTime}");
                }
            }
            return logEntries.Count > 0 ? string.Join("\n", logEntries) : $"No buy logs found for {iFoodName}.";
        }
        public static string _GetSpecificSellLog(string iFoodName)
        {
            List<string> logEntries = new List<string>();
            foreach (var log in _sellDataBase)
            {
                if (log._foodType._foodName.Equals(iFoodName, StringComparison.OrdinalIgnoreCase) ||
                    Array.Exists(log._foodType._alternativeNames, alt => alt.Equals(iFoodName, StringComparison.OrdinalIgnoreCase)))
                {
                    string userName = log._userName?.Length > _USER_NAME_LENGTH ? log._userName.Substring(0, _USER_NAME_LENGTH) : log._userName ?? "";
                    logEntries.Add($"{userName} => {log._foodType._foodName}: {log._foodPrice}, {log._orderTime}");
                }
            }
            return logEntries.Count > 0 ? string.Join("\n", logEntries) : $"No sell logs found for {iFoodName}.";
        }

        public static string _ShowFoodAverage(string iFoodName)
        {
            int count = 0;
            float totalPrice = 0;

            foreach (FoodLogStructure item in _sellDataBase)
            {
                if (item._foodType._foodName.Equals(iFoodName, StringComparison.OrdinalIgnoreCase) ||
                    Array.Exists(item._foodType._alternativeNames, alt => alt.Equals(iFoodName, StringComparison.OrdinalIgnoreCase)))
                {
                    count++;
                    totalPrice += item._foodPrice;
                }
            }

            if (count == 0)
            {
                return $"No entries found for {iFoodName}.";
            }

            float averagePrice = totalPrice / count;
            return $"{iFoodName} average price: {averagePrice:0.00}";
        }
    }

    public class _FoodType
    {
        public static List<_FoodType> _AllFoodTypes;
        public string _foodName;
        public float _maxPrice;
        public float _minPrice;
        public string[] _alternativeNames;

        public _FoodType(string iName, float iMaxPrice, float iMinPrice, params string[] iAlternatives)
        {
            _foodName = iName;
            _maxPrice = iMaxPrice;
            _minPrice = iMinPrice;
            _alternativeNames = iAlternatives;
            _AllFoodTypes ??= new List<_FoodType>();
            _AllFoodTypes.Add(this);
        }
        public override string ToString()
        {
            return _foodName;
        }
    }

    public enum _AllOrderTypes
    {
        buy, sell
    }

    public class FoodLogStructure
    {
        public _FoodType _foodType { get; set; }
        public _AllOrderTypes _orderType { get; set; }
        public float _foodPrice { get; set; }
        public string _userName { get; set; }
        public string _orderTime { get; set; }
    }

    public class _DataSet
    {
        public static void InitializeFoodTypes()
        {
            new _FoodType("kebab", 30, 10, "kobide", "kabab");
            new _FoodType("joje", 30, 10, "joje kabab");
        }
    }
}
