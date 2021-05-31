using System;

namespace XDataFlow.Repeaters
{
    public class ExpiredTaker<T> : Taker<T>
    {
        public DateTime ExpiredAfter { get; set; }
        
        public ExpiredTaker(Func<T> func) : base(func)
        {
        }

        public override T Take()
        {
            if (Value == null)
            {
                return base.Take();
            }
            
            return CalculatedAt <= ExpiredAfter ? Value : base.Take();
        }
    }
}