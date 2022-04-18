namespace SharpGlide.Routing
{
    public interface IRoute
    {
        string RoutingKey { get; set; }
        string Queue { get; set; }
        string Topic { get; set; }
    }
}