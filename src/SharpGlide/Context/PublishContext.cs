using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpGlide.Context.Abstractions;
using SharpGlide.Providers;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Routes;

namespace SharpGlide.Context
{
    public class PublishContext<TPublishData> : IPublishContext<TPublishData>
    {
        private readonly IVectorHeartBeatContext _heartBeatContext;
        
        public IDictionary<string, IPublishTunnel<TPublishData>> PublishTunnels { get; } =
            new Dictionary<string, IPublishTunnel<TPublishData>>();
        
        public Action<IEnumerable<TPublishData>> PublishDataPointer { get; set; }

        public PublishContext(IVectorHeartBeatContext heartBeatContext)
        {
            _heartBeatContext = heartBeatContext;
        }

        public void SetPublishFlow(Expression<Action<IEnumerable<TPublishData>>> flowExpr)
        {
            PublishDataPointer = flowExpr.Compile();
        }

        public void Publish(IEnumerable<TPublishData> data, IPublishRoute publishRoute)
        {
            throw new NotImplementedException();
        }
    }
}