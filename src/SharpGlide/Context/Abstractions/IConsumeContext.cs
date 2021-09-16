using System.Collections.Generic;
using SharpGlide.Tunnels;
using SharpGlide.Wrappers;

namespace SharpGlide.Context.Abstractions
{
    public interface IConsumeContext<TConsumeData> : IConsumeMetrics
    {
        IDictionary<string, IConsumeTunnel<TConsumeData>> ConsumeTunnels { get; }

        TConsumeWrapper AddConsumeWrapper<TConsumeWrapper>(TConsumeWrapper wrapper)
            where TConsumeWrapper : IWrapperWithOutput<TConsumeData>;

        void SetupBindingToTopic(IConsumeTunnel<TConsumeData> tunnel, string topicName, string queueName, string routingKey);
        
        IEnumerable<TConsumeData> ReadAndConsumeData();
        void Push(TConsumeData data);
        void Push(TConsumeData data, string tunnelKey);
        void PushRange(IEnumerable<TConsumeData> data);
    }
}