namespace SharpGlide.Tunnels.Routes
{
    public interface IConsumeRoute
    {
        string Topic { get; set; }
        string RoutingKey { get; set; }
        string Queue { get; set; }
        string Name { get; set; }
        IConsumeRoute CreateChild<T>(string routingKey) where T : IConsumeRoute, new();
        IConsumeRoute CreateChild<T>() where T : IConsumeRoute, new();
    }
}