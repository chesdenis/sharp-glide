using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.IO.Model;
using SharpGlide.Readers.Abstractions;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.IO.Readers
{
    public class FileContentWalker : PagedWalker<byte, IFileBytesRangeRequest>, IFileContentWalker
    {
        public FileContentWalker(
            Func<CancellationToken, PageInfo, IFileBytesRangeRequest, Action<IEnumerable<byte>>, Task> pagedWalkFunc,
            Func<CancellationToken, PageInfo, IFileBytesRangeRequest, Func<CancellationToken, IEnumerable<byte>, Task>,
                Task> pagedAsyncWalkFunc) : base(pagedWalkFunc, pagedAsyncWalkFunc)
        {
        }
    }
}