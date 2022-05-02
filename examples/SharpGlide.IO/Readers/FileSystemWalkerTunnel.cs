using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.IO.Model;
using SharpGlide.Tunnels.Read.Abstractions;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.IO.Readers
{
    public class FileSystemWalkerTunnel : WalkTunnel<FsEntryInfo, FsEntryInfo>, IFileSystemWalkTunnel
    {
        protected override async Task SingleWalkImpl(CancellationToken cancellationToken, FsEntryInfo request, Action<FsEntryInfo> callback)
        {
            var exceptions = new List<Exception>();
            
            await WalkDirectory(request, callback, null,exceptions);

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        protected override async Task SingleAsyncWalkImpl(CancellationToken cancellationToken, FsEntryInfo request, Func<CancellationToken, FsEntryInfo, Task> callback)
        {
            throw new NotImplementedException();
        }

        protected override async Task PagedWalkImpl(CancellationToken cancellationToken, PageInfo pageInfo, FsEntryInfo request, Action<IEnumerable<FsEntryInfo>> callback)
        {
            var exceptions = new List<Exception>();
            
            await WalkDirectory(request, null, callback,exceptions);

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        protected override async Task PagedAsyncWalkImpl(CancellationToken cancellationToken, PageInfo pageInfo, FsEntryInfo request, Func<CancellationToken, IEnumerable<FsEntryInfo>, Task> callback)
        {
            throw new NotImplementedException();
        }
        
        private async Task WalkDirectory(
            FsEntryInfo dirInfo,
            Action<FsEntryInfo> callbackSingle,
            Action<FsEntryInfo[]> callbackRange,
            List<Exception> exceptions)
        {
            try
            {
                var directory = new DirectoryInfo(dirInfo.FullName);
                
                var childrenFiles = directory.GetFiles();

                callbackRange?.Invoke(MapToFileAttributes(childrenFiles).ToArray());

                if (callbackSingle != null)
                {
                    WalkFiles(callbackSingle, childrenFiles, directory, exceptions);
                }

                var childrenDirectories = directory.GetDirectories();
                foreach (var childrenDirectory in childrenDirectories)
                {
                    await WalkDirectory(new FsEntryInfo()
                    {
                        FullName = childrenDirectory.FullName
                    }, callbackSingle, callbackRange, exceptions);
                }
            }
            catch (Exception e)
            {
                exceptions.Add(
                    new ArgumentException(
                        $"Unable to get details for folder {dirInfo.FullName}. Details: {e.Message}"));
            }
        }
        private void WalkFiles(
            Action<FsEntryInfo> callbackSingle, 
            FileInfo[] childrenFiles, 
            DirectoryInfo directory,
            List<Exception> exceptions)
        {
            foreach (var childrenFile in childrenFiles)
            {
                try
                {
                    callbackSingle(MapToFileAttributes(childrenFile));
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