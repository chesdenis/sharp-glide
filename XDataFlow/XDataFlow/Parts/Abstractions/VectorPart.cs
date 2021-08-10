using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Context;
using XDataFlow.Tunnels;

namespace XDataFlow.Parts.Abstractions
{
    public abstract class VectorPart<TConsumeData, TPublishData> : BasePart
    {
        public VectorPartContext<TConsumeData, TPublishData> VectorPartContext => 
            (VectorPartContext<TConsumeData, TPublishData>) this.Context;
        
        public IDictionary<string, IPublishTunnel<TPublishData>> PublishTunnels => 
            this.VectorPartContext.PublishController.PublishTunnels;
        
        public abstract Task ProcessAsync(
            TConsumeData data, 
            CancellationToken cancellationToken);

        public void Publish(TPublishData data)
        {
            foreach (var tunnelKey in PublishTunnels.Keys)
            {
                PublishTunnels[tunnelKey].Publish(data);
            }
        }
        
        public void Publish(TPublishData data, string routingKey)
        {
            foreach (var tunnelKey in PublishTunnels.Keys)
            {
                PublishTunnels[tunnelKey].Publish(data, routingKey);
            }
        }
        
        public void Publish(TPublishData data, string exchange, string routingKey)
        {
            foreach (var tunnelKey in PublishTunnels.Keys)
            {
                PublishTunnels[tunnelKey].Publish(data, exchange ,routingKey);
            }
        }
    }
}