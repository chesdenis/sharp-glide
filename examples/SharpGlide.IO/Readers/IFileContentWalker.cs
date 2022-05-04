using SharpGlide.IO.Model;
using SharpGlide.Readers.Interfaces;

namespace SharpGlide.IO.Readers
{
    public interface IFileContentWalker :
        IPagedWalker<byte, IFileBytesRangeRequest>,
        IPagedAsyncWalker<byte, IFileBytesRangeRequest>
    {
    }
}