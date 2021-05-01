using System;
using System.Collections.Concurrent;
using XDataFlow.Exceptions;

namespace XDataFlow.Tunnels.InMemory
{
    public class InMemoryConsumeTunnel<T> : ConsumeTunnel<T>
    {
        private readonly ConcurrentQueue<T> _queue;

        public InMemoryConsumeTunnel(ConcurrentQueue<T> queue)
        {
            _queue = queue;
        }

        public override Func<T> ConsumePointer()
        {
            return () =>
            {
                if (_queue.TryDequeue(out var result))
                {
                    return result;
                }
                
                throw new NoDataException();
            };
        }
    }
}