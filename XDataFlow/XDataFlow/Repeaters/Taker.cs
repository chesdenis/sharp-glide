using System;

namespace XDataFlow.Repeaters
{
    public class Taker<T>
    {
        public Func<T> Func;

        protected T Value;
 
        protected DateTime CalculatedAt { get; set; }

        public Taker(Func<T> func)
        {
            Func = func;
        }

        public virtual T Take()
        {
            CalculatedAt = DateTime.Now;
            Value = Func();
            return Value;
        }
    }
}