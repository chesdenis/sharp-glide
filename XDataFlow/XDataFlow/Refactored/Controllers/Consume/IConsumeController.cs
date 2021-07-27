using System;
using System.Collections.Generic;
using XDataFlow.Tunnels;
using XDataFlow.Wrappers;

namespace XDataFlow.Refactored.Controllers.Consume
{
    public interface IConsumeController<TConsumeData>
    {
        IDictionary<string, IConsumeTunnel<TConsumeData>> ConsumeTunnels { get; }

        int GetWaitingToConsumeAmount();

        TimeSpan GetEstimatedTime();

        int GetMessagesPerSecond();

        TConsumeWrapper AddConsumeWrapper<TConsumeWrapper>(TConsumeWrapper wrapper)
            where TConsumeWrapper : IWrapperWithOutput<TConsumeData>;

        void ConfigureListening(IConsumeTunnel<TConsumeData> tunnel, string topicName, string queueName, string routingKey);
    }
}