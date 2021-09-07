using System;

namespace DechFlow.Wrappers
{
    public interface IWrapperWithOutput<T>
    {
        Func<string, string, string, T> Wrap(Func<string, string, string, T> funcToWrap);
    }
}