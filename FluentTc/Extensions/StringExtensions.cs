using System.Text;

namespace FluentTc.Extensions
{
    public static class StringExtensions
    {
        public static string FromPascalToCapitalizedCase(this string value)
        {
            StringBuilder sb = new StringBuilder();

            int charCount = 0;
            foreach (char c in value)
            {
                charCount++;
                if (char.IsUpper(c) && charCount != value.Length && charCount != 1)
                {
                    sb.Append("_");
                }
                sb.Append(char.ToUpper(c));
            }
            return sb.ToString();
        }

        public static string FromPascalToCamelCase(this string value)
        {
            var len = value.Length;
            if (len > 0)
            {
                var sb = new StringBuilder();
                sb.Append(char.ToLower(value[0]));
                if (len > 1)
                    sb.Append(value.Substring(1, len - 1));
                return sb.ToString();
            }
            else
                return "";
        }
    }
}
