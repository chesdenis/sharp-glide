using System;

namespace SharpGlide.Context.Abstractions
{
    public interface IPointHeartBeatContext : IHeartBeatContext
    {
        DateTime LastActivity { get; set; }
    }
}