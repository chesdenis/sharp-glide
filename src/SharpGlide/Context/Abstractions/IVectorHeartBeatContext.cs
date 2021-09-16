using System;

namespace SharpGlide.Context.Abstractions
{
    public interface IVectorHeartBeatContext: IHeartBeatContext
    {
        DateTime LastPublishedAt { get; set; }

        DateTime LastConsumedAt { get; set; }
    }
}