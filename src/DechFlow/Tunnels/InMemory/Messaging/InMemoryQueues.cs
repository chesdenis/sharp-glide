using System.Collections.Concurrent;

namespace DechFlow.Tunnels.InMemory.Messaging
{
    public class InMemoryQueues : ConcurrentDictionary<string, InMemoryQueue<object>>
    {

    }
}