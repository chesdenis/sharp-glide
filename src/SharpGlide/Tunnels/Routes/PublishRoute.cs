namespace SharpGlide.Tunnels.Routes
{
    public class PublishRoute : IPublishRoute
    {
        public string Topic { get; set; }
        public string RoutingKey { get; set; }
        
        public IPublishRoute CreateChild(string routingKey)  
        {
            var route = new PublishRoute
            {
                Topic = this.Topic,
                RoutingKey = routingKey
            };
            return route;
        } 
        
        public IPublishRoute CreateChild() 
        {
            var route = new PublishRoute
            {
                Topic = this.Topic,
                RoutingKey = this.RoutingKey
            };
            return route;
        }
    }
}