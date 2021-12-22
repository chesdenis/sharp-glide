using System;
using System.Collections.Generic;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Tunnels.Routes.XRoutes
{
    public class XConsumeRoute : IXConsumeRoute
    {
        public override string ToString()
        {
            return $"{nameof(RoutingKey)}: {RoutingKey}, {nameof(Queue)}: {Queue}, {nameof(AssignedParts)}: {AssignedParts.Count}";
        }

        public List<IBasePart> AssignedParts { get; set; } = new List<IBasePart>();
        public string RoutingKey { get; set; }
        public string Queue { get; set; }
    }
}