using System;

namespace SharpGlide.TunnelWrappers.Abstractions
{
    public interface IConsumeWrapper<T>
    {
        Func<string, string, string, T> Wrap(Func<string, string, string, T> funcToWrap);
    }
}