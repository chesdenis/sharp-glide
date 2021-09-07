using System;

namespace DechFlow.Providers
{
    public interface IGuidProvider
    {
        Guid NewGuid();
    }
}