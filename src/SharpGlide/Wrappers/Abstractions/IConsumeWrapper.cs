using System;

namespace SharpGlide.Wrappers
{
    public interface IConsumeWrapper<T>
    {
        Func<string, string, string, T> Wrap(Func<string, string, string, T> funcToWrap);
    }
}