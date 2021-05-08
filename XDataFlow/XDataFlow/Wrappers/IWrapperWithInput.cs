using System;

namespace XDataFlow.Wrappers
{
    public interface IWrapperWithInput<T>
    {
        Action<T, string> Wrap(Action<T, string> actionToWrap, string routingKey);
    }
}