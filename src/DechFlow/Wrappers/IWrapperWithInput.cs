using System;

namespace DechFlow.Wrappers
{
    public interface IWrapperWithInput<T>
    {
        Action<T, string, string> Wrap(Action<T, string, string> actionToWrap, string exchange, string routingKey);
    }
}