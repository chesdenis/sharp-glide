using System.Collections.Generic;
using XDataFlow.Tunnels;
using XDataFlow.Wrappers;

namespace XDataFlow.Refactored.Controllers.Consume
{
    public interface IConsumeController<TConsumeData> : IConsumeMetrics
    {
        IDictionary<string, IConsumeTunnel<TConsumeData>> ConsumeTunnels { get; }

        TConsumeWrapper AddConsumeWrapper<TConsumeWrapper>(TConsumeWrapper wrapper)
            where TConsumeWrapper : IWrapperWithOutput<TConsumeData>;

        void ConfigureListening(IConsumeTunnel<TConsumeData> tunnel, string topicName, string queueName, string routingKey);
        IEnumerable<TConsumeData> ReadAndConsumeData();
        void PushDataToFirstTunnel(TConsumeData data);
        void PushDataToTunnel(TConsumeData data, string tunnelKey);
    }
}