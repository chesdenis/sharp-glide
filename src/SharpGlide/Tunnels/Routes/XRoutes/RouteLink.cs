using System;

namespace SharpGlide.Tunnels.Routes.XRoutes
{
    public class RouteLink
    {
        public override string ToString()
        {
            return $"\n\n{nameof(ConsumeRoute)}: {ConsumeRoute},\n{nameof(PublishRoute)}: {PublishRoute}";
        }

        public XConsumeRoute ConsumeRoute { get; set; }
        public XPublishRoute PublishRoute { get; set; }
    }
}