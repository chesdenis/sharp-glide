namespace SharpGlide.Tunnels.Routes
{
    public class ConsumeRoute : IConsumeRoute
    {
        public string Topic { get; set; }
        public string RoutingKey { get; set; }
        public string Queue { get; set; }
    }
}