namespace SharpGlide.Tunnels.Routes
{
    public class PublishRoute : IPublishRoute
    {
        public string Topic { get; set; }
        public string RoutingKey { get; set; }
    }
}