using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.IO.Model;
using SharpGlide.Readers.Abstractions;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.IO.Readers
{
    public class FileSystemWalker : Walker<FsEntryInfo, FsEntryInfo>, IFileSystemWalker
    {
        public FileSystemWalker(
            Func<CancellationToken, FsEntryInfo, Action<FsEntryInfo>, Task> singleWalkFunc,
            Func<CancellationToken, FsEntryInfo, Func<CancellationToken, FsEntryInfo, Task>, Task> singleAsyncWalkFunc,
            Func<CancellationToken, PageInfo, FsEntryInfo, Action<IEnumerable<FsEntryInfo>>, Task> pagedWalkFunc,
            Func<CancellationToken, PageInfo, FsEntryInfo, Func<CancellationToken, IEnumerable<FsEntryInfo>, Task>,
                Task> pagedAsyncWalkFunc) 
            : base(singleWalkFunc, singleAsyncWalkFunc, pagedWalkFunc, pagedAsyncWalkFunc)
        {
        }
    }
}