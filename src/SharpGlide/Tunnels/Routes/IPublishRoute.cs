namespace SharpGlide.Tunnels.Routes
{
    public interface IPublishRoute
    {
        string Topic { get; set; }
        string RoutingKey { get; set; }
        IPublishRoute CreateChild(string routingKey);
        IPublishRoute CreateChild();
    }
}