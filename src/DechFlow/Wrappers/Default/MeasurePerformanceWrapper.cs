using System;
using System.Diagnostics;

namespace DechFlow.Wrappers.Default
{
    public class MeasurePerformanceWrapper : IWrapper
    {
        private readonly Stopwatch _sw = new Stopwatch();

        public double TotalSeconds => _sw.Elapsed.TotalSeconds;

        public Action Wrap(Action actionToWrap)
        {
            return () =>
            {
                _sw.Reset();
                _sw.Start();

                actionToWrap();

                _sw.Stop();
            };
        }
    }
}