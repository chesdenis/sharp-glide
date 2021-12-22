using System;
using System.Collections.Generic;
using SharpGlide.Parts.Abstractions;

namespace SharpGlide.Tunnels.Routes.XRoutes
{
    public class XPublishRoute : IXPublishRoute
    {
        public override string ToString()
        {
            return $"{nameof(Topic)}: {Topic}, {nameof(RoutingKey)}: {RoutingKey}, {nameof(AssignedParts)}: {AssignedParts.Count}";
        }

        public List<IBasePart> AssignedParts { get; set; } = new List<IBasePart>();
        public string Topic { get; set; }

        public string RoutingKey { get; set; }
    }
}