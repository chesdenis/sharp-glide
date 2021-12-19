using System;
using System.Collections.Generic;
using System.Linq;
using SharpGlide.Context.Abstractions;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Routes;

namespace SharpGlide.Context
{
    public class ConsumeContext<TConsumeData> : IConsumeContext<TConsumeData>
    {
        public IDictionary<string, IConsumeTunnel<TConsumeData>> ConsumeTunnels { get; } =
            new Dictionary<string, IConsumeTunnel<TConsumeData>>();

        // TODO: possible move this and related to flow configuration
        public void SetupBindingToTopic(IConsumeTunnel<TConsumeData> tunnel, IConsumeRoute consumeRoute)
        {
            tunnel.SetupInfrastructure(consumeRoute);
            
            ConsumeTunnels.Add(Guid.NewGuid().ToString("B"), tunnel);
        }

        public void TakeAndConsume<TConsumeRoute>(
            TConsumeRoute consumeRoute, 
            params TConsumeData[] data) 
            where TConsumeRoute : IConsumeRoute => 
            TakeAndConsume(ConsumeTunnels.First().Key, consumeRoute, data);

        public void TakeAndConsume<TConsumeRoute>(
            string tunnelKey, 
            TConsumeRoute consumeRoute, 
            params TConsumeData[] data)
            where TConsumeRoute : IConsumeRoute
        {
            var consumeTunnel = ConsumeTunnels
                .First(f=>f.Key == tunnelKey)
                .Value;
            
            foreach (var consumeData in data)
            {
                consumeTunnel.TakeAndConsume(consumeData, consumeRoute);
            }
        }
        
        // TODO: make it async
        public IEnumerable<TConsumeData> Consume()
        {
            var tunnels = ConsumeTunnels;

            foreach (var tunnelKey in tunnels.Keys)
            {
                // TODO: make it awaitable
                var consumeTunnel = tunnels[tunnelKey];

                foreach (var route in consumeTunnel.Routes.Keys)
                {
                    var consumeRoute = consumeTunnel.Routes[route];
                    var consumeData = consumeTunnel.Consume(consumeRoute);
                    yield return consumeData;
                }
            }
        }
    }
}