using System;

namespace SharpGlide.Context.HeartBeat
{
    public interface IPointHeartBeatContext : IHeartBeatContext
    {
        DateTime LastActivity { get; set; }
    }
}