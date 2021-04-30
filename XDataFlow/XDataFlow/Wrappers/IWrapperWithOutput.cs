using System;

namespace XDataFlow.Wrappers
{
    public interface IWrapperWithOutput<T>
    {
        Func<T> Wrap(Func<T> funcToWrap);
    }
}