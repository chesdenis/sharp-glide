using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Routes.XRoutes;

namespace SharpGlide.Context.Abstractions
{
    public interface IPublishContext<TPublishData>
    { 
        IDictionary<string, IPublishTunnel<TPublishData>> PublishTunnels { get; }

        void SetPublishFlow(
            Expression<Action<IEnumerable<TPublishData>,
                IXPublishRoute>> flowExpr);
        
        void Publish(IEnumerable<TPublishData> data, IXPublishRoute publishRoute);
    }
}