using System.Collections.Generic;
using DechFlow.Tunnels;
using DechFlow.Wrappers;

namespace DechFlow.Context
{
    public interface IConsumeContext<TConsumeData> : IConsumeMetrics
    {
        IDictionary<string, IConsumeTunnel<TConsumeData>> ConsumeTunnels { get; }

        TConsumeWrapper AddConsumeWrapper<TConsumeWrapper>(TConsumeWrapper wrapper)
            where TConsumeWrapper : IWrapperWithOutput<TConsumeData>;

        void SetupBindingToTopic(IConsumeTunnel<TConsumeData> tunnel, string topicName, string queueName, string routingKey);
        
        IEnumerable<TConsumeData> ReadAndConsumeData();
        void ConsumeData(TConsumeData data);
        void ConsumeData(TConsumeData data, string tunnelKey);
    }
}