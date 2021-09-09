using System;

namespace SharpGlide.Fields
{
    public class ExpiredField<T> : Field<T>
    {
        public DateTime ExpiredAfter { get; set; }
        
        public ExpiredField(Func<T> func) : base(func)
        {
        }

        protected override T GetValue()
        {
            if (Value == null)
            {
                return base.GetValue();
            }
            
            return CalculatedAt <= ExpiredAfter ? Value : base.GetValue();
        }
    }
}