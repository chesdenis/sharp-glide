namespace SharpGlide.Tunnels.Routes
{
    public class ConsumeRoute : IConsumeRoute
    {
        public string Topic { get; set; }
        public string RoutingKey { get; set; }
        public string Queue { get; set; }
        public string Name { get; set; }

        public ConsumeRoute()
        {
            Name = $"CR_{this.GetType().Name}";
        }

        public static ConsumeRoute Default => new ConsumeRoute
        {
            Topic = "DefaultTopic",
            RoutingKey = "#",
            Queue = "DefaultQueue",
            Name = "Default"
        };

        public IConsumeRoute CreateChild<T>(string routingKey) where T : IConsumeRoute, new()
        {
            var route = new T
            {
                Topic = this.Topic,
                Queue = this.Queue,
                RoutingKey = routingKey
            };
            return route;
        }

        public IConsumeRoute CreateChild<T>() where T : IConsumeRoute, new()
        {
            var route = new T
            {
                Topic = this.Topic,
                RoutingKey = this.RoutingKey,
                Queue = this.Queue
            };
            return route;
        }
    }
}