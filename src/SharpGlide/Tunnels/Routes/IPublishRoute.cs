namespace SharpGlide.Tunnels.Routes
{
    public interface IPublishRoute
    {
        string Name { get; set; }
        string Topic { get; set; }
        string RoutingKey { get; set; }
        IPublishRoute CreateChild<T>(string routingKey) where T : IPublishRoute, new();
        IPublishRoute CreateChild<T>() where T : IPublishRoute, new();
    }
}