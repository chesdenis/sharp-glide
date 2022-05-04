using SharpGlide.IO.Model;
using SharpGlide.Tunnels.Read.Interfaces;

namespace SharpGlide.IO.Tunnels
{
    public interface IFileContentWalkTunnel :
        IPagedWalkTunnel<byte, IFileBytesRangeRequest>,
        IPagedAsyncWalkTunnel<byte, IFileBytesRangeRequest>
    {
        
    }
}