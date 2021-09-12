using System;

namespace SharpGlide.Wrappers
{
    public interface IWrapperWithInput<T>
    {
        Action<T, string, string> Wrap(Action<T, string, string> actionToWrap, string exchange, string routingKey);
    }
}