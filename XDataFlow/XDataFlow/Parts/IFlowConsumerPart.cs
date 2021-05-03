using System;
using System.Collections.Generic;
using XDataFlow.Tunnels;

namespace XDataFlow.Parts
{
    public interface IFlowConsumerPart<TConsumeData>
    {
        IDictionary<string, IConsumeTunnel<TConsumeData>> ConsumeTunnels { get; }
    }
}