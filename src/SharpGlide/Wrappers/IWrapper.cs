using System;

namespace SharpGlide.Wrappers
{
    public interface IWrapper
    {
        Action Wrap(Action actionToWrap);
    }
}