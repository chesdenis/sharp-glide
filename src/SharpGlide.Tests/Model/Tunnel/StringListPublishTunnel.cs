using System;
using System.Collections.Generic;
using SharpGlide.Extensions;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Routes;

namespace SharpGlide.Tests.Model.Tunnel
{
    public class StringListPublishTunnel : PublishTunnel<string>
    {
        public readonly List<string> PublishedData = new();

        public override Action<string, IPublishRoute> PublishPointer()
        {
            return (data, _) => { this.PublishedData.Add(data); };
        }

        public override void SetupInfrastructure(IPublishRoute publishRoute)
        {
        }
    }
}