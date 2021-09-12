using System;

namespace SharpGlide.Context.HeartBeat
{
    public interface IVectorHeartBeatContext: IHeartBeatContext
    {
        DateTime LastPublishedAt { get; set; }

        DateTime LastConsumedAt { get; set; }
    }
}