using SharpGlide.Model;
using SharpGlide.Parts.Abstractions;
using SharpGlide.Tunnels.Abstractions;

namespace SharpGlide.Context.Abstractions
{
    public interface IHeartBeatContext
    {
        int IdleTimeoutMs { get; set; }

        bool Idle { get; }

        bool Failed { get; }
        
        IPublishTunnel<HeartBeat> HeartBeatTunnel { get; set; }

        void Collect();

        HeartBeat Get();
    }
}