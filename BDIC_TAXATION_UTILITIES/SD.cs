using System.ComponentModel;
using System.Reflection;

namespace BDIC_TAXATION_UTILITIES
{
    public static class SD
    {
        public const string Role_Admin = "Admin";
        public const string Role_SubAdmin = "Sub Admin";
        public const string Role_Individual = "Individual";
        public const string Role_Business = "Business";
        public const string Role_Consultant = "Consultant";
        public const string Role_MDAs = "Ministry";
        public const int Tax_percent = 7;
        public enum UserStatus { Active, Suspended, Banned }
        public enum PromotionType { LevelAdvancement, StepAdvancement }

        //OBJECTIONS STATUS
        public const string Objection_Review = "Under review";
        public const string Objection_Progress = "In Progress";
        public const string Objection_Resolved = "Resolved";
        public const string Objection_Closed = "Closed";
        public enum ObjectionStatus
        {
            [Description("Under review")]
            Review,
            [Description("In Progress")]
            Progress,
            [Description("Resolved")]
            Resolved,
            [Description("Closed")]
            Closed
        }
        //var statusString = ObjectionStatus.Review.ToDescriptionString();
        public static string ToObjectionStatusText(this ObjectionStatus val)
        {
            var attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : val.ToString();
        }
        //string description = GetObjectionStatusDescription(0);
        public static string GetObjectionStatusDescription(int statusNumber)
        {
            if (Enum.IsDefined(typeof(ObjectionStatus), statusNumber))
            {
                ObjectionStatus status = (ObjectionStatus)statusNumber;
                FieldInfo field = status.GetType().GetField(status.ToString());
                DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();
                return attribute?.Description ?? status.ToString();
            }
            throw new ArgumentOutOfRangeException(nameof(statusNumber), "Invalid status number");
        }
        public static string GenerateCodeFromId(int id, string substring, int digits)
        {
            const int primeNumber = 104729;
            long transformedValue = (id + primeNumber);
            string code = transformedValue.ToString();
            if (code.Length < digits)
            {
                code = code.PadLeft(digits, '0');
            }
            else if (code.Length > digits)
            {
            // Take last N digits if too long
                code = code.Substring(code.Length - digits);
            }
            return substring+code;
        }
        public static string TruncateStringToMaxLength(string input, int maxLength)
        {
            return input.Length > maxLength ? input[..maxLength] : input;
        }
        public static int ToIndex(int number)
        {
            var index = number - 1;
            return index < 0 ? 0 : index;
        }
        public static string ToNaira(double money)
        {
            char naira = (char)8358;
            string Money;
            Money = money.ToString("c");
            return Money.Replace('$', naira);
        }
        public static decimal CalculatePercent(decimal percent, decimal amount)
        {
            return (percent / 100m) * amount;
        }
        public static string GenerateUniqueNumber()
        {
            string dateTimeString = DateTime.Now.ToString("yyyyMMddHHmmss");
            Random random = new Random();
            string randomAlphabets = new string(Enumerable.Range(0, 4).Select(_ => (char)random.Next('A', 'Z' + 1)).ToArray());
            return dateTimeString + randomAlphabets;
        }
        public static string TrackNumber()
        {
            string dateTimeString = DateTime.Now.ToString("yyyyMMddHHmm");
            Random random = new Random();
            string randomAlphabets = new string(Enumerable.Range(0, 4).Select(_ => (char)random.Next('A', 'Z' + 1)).ToArray());
            return dateTimeString + randomAlphabets;
        }
        public static string GenerateUniqueNumberWithInnitials(string i)
        {
            string dateTimeString = DateTime.Now.ToString("yyyyMMddHHmm");
            Random random = new Random();
            string randomAlphabets = new string(Enumerable.Range(0, 4).Select(_ => (char)random.Next('A', 'Z' + 1)).ToArray());
            return i+dateTimeString + randomAlphabets;
        }
        public static string ExtractNameAndCV(string name)
        {
            string[] parts = name.Split(new[] { '_' }, 2);
            return parts.Length > 1 ? parts[1] : name;
        }
    }
}
