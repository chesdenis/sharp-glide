using System;
using System.Threading.Tasks;

namespace SharpGlide.Fields
{
    public class RandomWaitBeforeRead<T> : Field<T>
    {
        private readonly TimeSpan _minDelay;
        private readonly TimeSpan _maxDelay;
        private readonly TimeSpan _approxDelay;
        
        private Random _random = new Random();
        
        public RandomWaitBeforeRead(Func<T> func, TimeSpan minDelay, TimeSpan maxDelay) : base(func)
        {
            _minDelay = minDelay;
            _maxDelay = maxDelay;
        }

        protected override T GetValue()
        {
            var dynamicWait = _random.Next(
                (int) _minDelay.TotalMilliseconds, 
                (int) _maxDelay.TotalMilliseconds);

            Task.Delay(dynamicWait).Wait();
            
            return base.GetValue();
        }
    }
}