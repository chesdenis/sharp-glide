using System.Collections.Generic;
using XDataFlow.Wrappers;

namespace XDataFlow.Tunnels
{
    public interface IConsumeTunnel<T>
    {
        IList<IWrapperWithOutput<T>> OnConsumeWrappers { get; }

        T Consume();
    }
}