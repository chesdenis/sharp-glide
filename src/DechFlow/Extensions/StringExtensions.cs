using System.Linq;

namespace DechFlow.Extensions
{
    public static class StringExtensions
    {
        public static string IndentLeft(this string text, char indentChar, int indentation)
        {
            return $"{string.Join(string.Empty, Enumerable.Repeat(indentChar, indentation))}{text}";
        }
    }
}