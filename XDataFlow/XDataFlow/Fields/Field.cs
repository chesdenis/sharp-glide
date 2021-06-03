using System;

namespace XDataFlow.Fields
{
    public class Field<T>
    {
        public Func<T> Func;

        protected T Value;
 
        protected DateTime CalculatedAt { get; set; }

        public Field(Func<T> func)
        {
            Func = func;
        }

        public virtual T GetValue()
        {
            CalculatedAt = DateTime.Now;
            Value = Func();
            return Value;
        }
    }
}