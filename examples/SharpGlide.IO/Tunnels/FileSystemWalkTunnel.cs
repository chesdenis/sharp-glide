using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.IO.Model;
using SharpGlide.Tunnels.Read.Abstractions;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.IO.Tunnels
{
    public class FileSystemWalkTunnel : WalkTunnel<FsEntryInfo, FsEntryInfo>, IFileSystemWalkTunnel
    {
        protected override async Task SingleWalkImpl(CancellationToken cancellationToken, FsEntryInfo request,
            Action<FsEntryInfo> callback)
        {
            var exceptions = new List<Exception>();

            await WalkDirectory(request,
                null,
                null,
                callback,
                null,
                exceptions,
                cancellationToken);

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        protected override async Task SingleAsyncWalkImpl(CancellationToken cancellationToken, FsEntryInfo request,
            Func<CancellationToken, FsEntryInfo, Task> callback)
        {
            var exceptions = new List<Exception>();

            await WalkDirectory(request,
                callback,
                null,
                null,
                null,
                exceptions,
                cancellationToken);

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        protected override async Task PagedWalkImpl(CancellationToken cancellationToken, PageInfo pageInfo,
            FsEntryInfo request, Action<IEnumerable<FsEntryInfo>> callback)
        {
            var exceptions = new List<Exception>();

            await WalkDirectory(request,
                null,
                null,
                null,
                callback,
                exceptions,
                cancellationToken);

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        protected override async Task PagedAsyncWalkImpl(CancellationToken cancellationToken, PageInfo pageInfo,
            FsEntryInfo request, Func<CancellationToken, IEnumerable<FsEntryInfo>, Task> callback)
        {
            var exceptions = new List<Exception>();

            await WalkDirectory(request,
                null,
                callback,
                null,
                null,
                exceptions,
                cancellationToken);

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        private async Task WalkDirectory(
            FsEntryInfo dirInfo,
            Func<CancellationToken, FsEntryInfo, Task> asyncCallbackSingle,
            Func<CancellationToken, FsEntryInfo[], Task> asyncCallbackRange,
            Action<FsEntryInfo> callbackSingle,
            Action<FsEntryInfo[]> callbackRange,
            List<Exception> exceptions,
            CancellationToken cancellationToken)
        {
            try
            {
                var directory = new DirectoryInfo(dirInfo.FullName);

                var childrenFiles = directory.GetFiles();

                var fsEntryInfos = MapToFileAttributes(childrenFiles).ToArray();

                if (asyncCallbackRange != null)
                {
                    await asyncCallbackRange(cancellationToken, fsEntryInfos);
                }

                callbackRange?.Invoke(fsEntryInfos);

                await WalkFiles(
                    asyncCallbackSingle,
                    callbackSingle,
                    childrenFiles,
                    directory,
                    exceptions,
                    cancellationToken);

                var childrenDirectories = directory.GetDirectories();
                foreach (var childrenDirectory in childrenDirectories)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    await WalkDirectory(new FsEntryInfo()
                        {
                            FullName = childrenDirectory.FullName
                        },
                        asyncCallbackSingle,
                        asyncCallbackRange,
                        callbackSingle,
                        callbackRange,
                        exceptions,
                        cancellationToken);
                }
            }
            catch (Exception e)
            {
                exceptions.Add(
                    new ArgumentException(
                        $"Unable to get details for folder {dirInfo.FullName}. Details: {e.Message}"));
            }
        }

        private async Task WalkFiles(
            Func<CancellationToken, FsEntryInfo, Task> asyncCallbackSingle,
            Action<FsEntryInfo> callbackSingle,
            FileInfo[] childrenFiles,
            DirectoryInfo directory,
            List<Exception> exceptions,
            CancellationToken cancellationToken)
        {
            foreach (var childrenFile in childrenFiles)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    var fileAttributes = MapToFileAttributes(childrenFile);

                    if (asyncCallbackSingle != null)
                    {
                        await asyncCallbackSingle(cancellationToken, fileAttributes);
                    }

                    callbackSingle?.Invoke(fileAttributes);
                }
                catch (Exception e)
                {
                    exceptions.Add(
                        new ArgumentException(
                            $"Unable to read data from folder {directory.FullName}. Details: {e.Message}"));
                }
            }
        }

        private static IEnumerable<FsEntryInfo> MapToFileAttributes(IEnumerable<FileInfo> childrenFiles) =>
            childrenFiles.Select(MapToFileAttributes);

        private static FsEntryInfo MapToFileAttributes(FileInfo childrenFile)
        {
            return new FsEntryInfo
            {
                FullName = childrenFile.FullName,
                Name = childrenFile.Name,
                Size = childrenFile.Length
            };
        }
    }
}