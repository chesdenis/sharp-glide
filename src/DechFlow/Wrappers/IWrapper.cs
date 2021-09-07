using System;

namespace DechFlow.Wrappers
{
    public interface IWrapper
    {
        Action Wrap(Action actionToWrap);
    }
}