using System;

namespace XDataFlow.Repeaters
{
    public static class TakerExtensions
    {
        public static ExpiredTaker<T> ExpireAfter<T>(this Taker<T> taker, TimeSpan timeSpan)
        {
            return new ExpiredTaker<T>(taker.Func)
            {
                ExpiredAfter = DateTime.Now.Add(timeSpan)
            };
        }
        
        public static Taker<T> From<T>(this Func<T> func)
        {
            return new Taker<T>(func);
        }
    }
}