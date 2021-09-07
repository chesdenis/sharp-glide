using System;

namespace DechFlow.Fields
{
    public static class FieldExtensions
    {
        public static RandomWaitBeforeRead<T> RandomWaitBeforeRead<T>(
            this Field<T> field,
            TimeSpan minWait,
            TimeSpan maxWait)
        {
            return new RandomWaitBeforeRead<T>(field.Func, minWait, maxWait);
        }
        
        public static ExpiredField<T> ExpireAfter<T>(this Field<T> field, TimeSpan timeSpan)
        {
            return new ExpiredField<T>(field.Func)
            {
                ExpiredAfter = DateTime.Now.Add(timeSpan)
            };
        }
        
        public static Field<T> From<T>(this Func<T> func)
        {
            return new Field<T>(func);
        }
    }
}