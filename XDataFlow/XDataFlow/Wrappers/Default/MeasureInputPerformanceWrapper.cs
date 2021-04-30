using System;
using System.Diagnostics;

namespace XDataFlow.Wrappers.Default
{
    public class MeasureInputPerformanceWrapper<T> : IWrapperWithInput<T>
    {
        private readonly Stopwatch _sw = new Stopwatch();
        
        public double TotalSeconds => _sw.Elapsed.TotalSeconds;

        public Action<T> Wrap(Action<T> actionToWrap)
        {
            return (arg) =>
            {
                _sw.Reset();
                _sw.Start();
                
                actionToWrap(arg);
                
                _sw.Stop();
            };
        }
    }
}