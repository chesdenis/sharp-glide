using System;

namespace XDataFlow.Fields
{
    public class ExpiredField<T> : Field<T>
    {
        public DateTime ExpiredAfter { get; set; }
        
        public ExpiredField(Func<T> func) : base(func)
        {
        }

        public override T GetValue()
        {
            if (Value == null)
            {
                return base.GetValue();
            }
            
            return CalculatedAt <= ExpiredAfter ? Value : base.GetValue();
        }
    }
}