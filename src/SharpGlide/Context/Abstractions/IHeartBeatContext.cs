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
        
        IPublishTunnel<string> HeartBeatTunnel { get; set; }

        void Collect();

        string ReportAsXml(IBasePart startPart);
    }
}