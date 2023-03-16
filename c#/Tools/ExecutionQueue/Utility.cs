using System.Text.RegularExpressions;

namespace ExecutionQueue
{
    public static class Utility
    {
        public static string RemoveIntegers(this string input)
        {
            return Regex.Replace(input, @"[\d-]", string.Empty);
        }
    }
}
