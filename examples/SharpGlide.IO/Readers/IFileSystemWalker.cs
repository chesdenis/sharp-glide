using SharpGlide.IO.Model;
using SharpGlide.Readers.Interfaces;

namespace SharpGlide.IO.Readers
{
    public interface IFileSystemWalker :
        ISingleWalker<FsEntryInfo, FsEntryInfo>,
        IPagedWalker<FsEntryInfo, FsEntryInfo>,
        ISingleAsyncWalker<FsEntryInfo, FsEntryInfo>,
        IPagedAsyncWalker<FsEntryInfo, FsEntryInfo>
    {
    }
}