using System;

namespace XDataFlow.Wrappers
{
    public interface IWrapperWithInput<T>
    {
        Action<T> Wrap(Action<T> actionToWrap);
    }
}