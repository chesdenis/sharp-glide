using SharpGlide.IO.Model;
using SharpGlide.Tunnels.Read.Interfaces;

namespace SharpGlide.IO.Tunnels
{
    public interface IFileSystemWalkTunnel :
        ISingleWalkTunnel<FsEntryInfo, FsEntryInfo>,
        IPagedWalkTunnel<FsEntryInfo, FsEntryInfo>,
        ISingleAsyncWalkTunnel<FsEntryInfo, FsEntryInfo>,
        IPagedAsyncWalkTunnel<FsEntryInfo, FsEntryInfo>
    {
        
    }
}