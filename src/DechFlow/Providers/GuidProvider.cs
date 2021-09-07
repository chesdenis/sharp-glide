using System;

namespace DechFlow.Providers
{
    public class GuidProvider : IGuidProvider
    {
        public Guid NewGuid() => Guid.NewGuid();
    }
}