namespace SharpGlide.Extensions
{
    public static class StringExtensions
    {
        public static string CutIfMoreCharacters(this string input, int maxLength = 50)
        {
            if (maxLength < 0)
            {
                return input;
            }
            
            if (input.Length < maxLength)
            {
                return input;
            }

            var delta =  maxLength / 2;

            return $"{input.Substring(0, delta)}...{input.Substring(input.Length - delta, delta)}";
        }
    }
}