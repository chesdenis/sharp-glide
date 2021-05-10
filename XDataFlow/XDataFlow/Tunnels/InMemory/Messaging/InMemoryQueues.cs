using System.Collections.Concurrent;

namespace XDataFlow.Tunnels.InMemory.Messaging
{
    public class InMemoryQueues : ConcurrentDictionary<string, InMemoryQueue<object>>
    {

    }
}