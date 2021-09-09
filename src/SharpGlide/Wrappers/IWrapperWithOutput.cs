using System;

namespace SharpGlide.Wrappers
{
    public interface IWrapperWithOutput<T>
    {
        Func<string, string, string, T> Wrap(Func<string, string, string, T> funcToWrap);
    }
}