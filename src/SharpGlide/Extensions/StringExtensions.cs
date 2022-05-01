using System;
using System.Collections.Generic;
using System.Linq;

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

            var delta = maxLength / 2;

            return $"{input.Substring(0, delta)}...{input.Substring(input.Length - delta, delta)}";
        }

        public static string BindUriArgs(this string input, params string[] keyValueArgs)
        {
            var dictionary = new Dictionary<string, string>();

            for (int i = 1; i < keyValueArgs.Length; i += 2)
            {
                dictionary.Add(keyValueArgs[i - 1], keyValueArgs[i]);
            }

            return input + "?" + string.Join("&", dictionary.Select(s => $"{s.Key}={s.Value}").ToArray());
        }

        public static Dictionary<string, string> GetUrlEncodedDict(this string input, char startsFromChar)
        {
            var retVal = new Dictionary<string, string>();

            try
            {
                input = input.Substring(input.IndexOf(startsFromChar) + 1);

                var rows = input.Split('&');

                foreach (var row in rows)
                {
                    var fields = row.Split('=');

                    retVal.Add(fields[0], fields[1]);
                }
            }
            catch
            {
                // ignored
            }

            return retVal;
        }
    }
}