namespace SharpGlide.Tunnels.Routes
{
    public interface IConsumeRoute
    {
        string Topic { get; set; }
        string RoutingKey { get; set; }
        string Queue { get; set; }
    }
}