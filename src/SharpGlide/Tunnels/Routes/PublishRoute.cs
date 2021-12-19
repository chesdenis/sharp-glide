namespace SharpGlide.Tunnels.Routes
{
    public class PublishRoute : IPublishRoute
    {
        public string Name { get; set; }
        public string Topic { get; set; }
        public string RoutingKey { get; set; }

        public PublishRoute()
        {
            Name = $"PR_{this.GetType().Name}";
        }
        
        public static PublishRoute Default => new PublishRoute
        {
            Topic = "DefaultTopic",
            RoutingKey = "#",
            Name = "Default"
        };

        public IPublishRoute CreateChild<T>(string routingKey) where T : IPublishRoute, new()
        {
            var route = new T
            {
                Topic = this.Topic,
                RoutingKey = routingKey
            };
            return route;
        }

        public IPublishRoute CreateChild<T>() where T : IPublishRoute, new()
        {
            var route = new T
            {
                Topic = this.Topic,
                RoutingKey = this.RoutingKey
            };
            return route;
        }
    }
}