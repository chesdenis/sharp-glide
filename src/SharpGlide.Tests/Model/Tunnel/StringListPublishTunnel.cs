using System;
using System.Collections.Generic;
using SharpGlide.Extensions;
using SharpGlide.Model;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Routes;

namespace SharpGlide.Tests.Model.Tunnel
{
    public class StringListPublishTunnel : PublishTunnel<HeartBeat>
    {
        public readonly List<string> PublishedData = new();

        public override Action<HeartBeat, IPublishRoute> PublishPointer()
        {
            return (data, _) => { this.PublishedData.Add(data.AsXml()); };
        }

        public override void SetupInfrastructure(IPublishRoute publishRoute)
        {
        }
    }
}