using System;

namespace XDataFlow.Wrappers
{
    public interface IWrapperWithOutput<T>
    {
        Func<string, string, string, T> Wrap(Func<string, string, string, T> funcToWrap);
    }
}