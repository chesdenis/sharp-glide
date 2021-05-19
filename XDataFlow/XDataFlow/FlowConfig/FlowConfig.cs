using System.Collections.Generic;

namespace XDataFlow.FlowConfig
{
    public class FlowConfig
    {
        public HashSet<TopicConfigNode> Topics { get; } = new HashSet<TopicConfigNode>();
        public HashSet<QueueConfigNode> Queues { get; } = new HashSet<QueueConfigNode>();
        public HashSet<RouteConfigNode> Routes { get; } = new HashSet<RouteConfigNode>();

        public TopicConfigNode LastModifiedTopic { get; set; }
       
        public RouteConfigNode LastModifiedRoute { get; set; }

       
    }
}