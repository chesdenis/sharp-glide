using System;

namespace XDataFlow.Wrappers
{
    public interface IWrapper
    {
        Action Wrap(Action actionToWrap);
    }
}