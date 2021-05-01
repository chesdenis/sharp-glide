using System;
using System.Collections.Concurrent;

namespace XDataFlow.Tunnels.InMemory
{
    public class InMemoryPublishTunnel<T> : PublishTunnel<T>
    {
        private readonly ConcurrentQueue<T> _queue;

        public InMemoryPublishTunnel(ConcurrentQueue<T> queue)
        {
            _queue = queue;
        }

        public override Action<T> PublishPointer()
        {
            return data => _queue.Enqueue(data);
        }
    }
}