using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharpGlide.Extensions;
using SharpGlide.Tunnels.Read.Abstractions;
using SharpGlide.Tunnels.Read.Model;
 
namespace SharpGlide.FilesToCloudUploader.Tunnels
{
    public class FolderContentsWalkTunnel : WalkTunnel<FolderContentsWalkTunnel.FileMetadata, FolderContentsWalkTunnel.DirectoryMetadata>
    {
        private readonly ILogger<FolderContentsWalkTunnel> _logger;

        public FolderContentsWalkTunnel(ILogger<FolderContentsWalkTunnel> logger)
        {
            _logger = logger;
        }
        
        protected override async Task SingleWalkImpl(CancellationToken cancellationToken, DirectoryMetadata request, Action<FileMetadata> callback)
        {
            await WalkDirectory(request, callback, null);
        }

        protected override async Task SingleAsyncWalkImpl(CancellationToken cancellationToken, DirectoryMetadata request, Func<CancellationToken, FileMetadata, Task> callback)
        {
            throw new NotImplementedException();
        }

        protected override async Task PagedWalkImpl(CancellationToken cancellationToken, PageInfo pageInfo, DirectoryMetadata request, Action<IEnumerable<FileMetadata>> callback)
        {
            await WalkDirectory(request,null, callback);
        }

        protected override async Task PagedAsyncWalkImpl(CancellationToken cancellationToken, PageInfo pageInfo, DirectoryMetadata request,
            Func<CancellationToken, IEnumerable<FileMetadata>, Task> callback)
        {
            throw new NotImplementedException();
        }
        
        private async Task WalkDirectory(DirectoryMetadata request,
            Action<FileMetadata> callbackSingle,
            Action<FileMetadata[]> callbackRange)
        {
            try
            {
                _logger.LogInformation("Looking into {folderPath}", request.FullName.CutIfMoreCharacters());
                var directory = new DirectoryInfo(request.FullName);
                
                var childrenFiles = directory.GetFiles();

                callbackRange?.Invoke(MapToFileAttributes(childrenFiles).ToArray());

                if (callbackSingle != null)
                {
                    WalkFiles(callbackSingle, childrenFiles, directory);
                }

                var childrenDirectories = directory.GetDirectories();
                foreach (var childrenDirectory in childrenDirectories)
                {
                    await WalkDirectory(new DirectoryMetadata()
                    {
                        FullName = childrenDirectory.FullName
                    }, callbackSingle, callbackRange);
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Unable to get details for folder {folderPath}", request.FullName);
            }
        }

        private void WalkFiles(Action<FileMetadata> callbackSingle, FileInfo[] childrenFiles, DirectoryInfo directory)
        {
            foreach (var childrenFile in childrenFiles)
            {
                try
                {
                    callbackSingle(MapToFileAttributes(childrenFile));
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, "Unable to read data from folder {folderPath}", directory.FullName);
                }
            }
        }

        private static IEnumerable<FileMetadata> MapToFileAttributes(IEnumerable<FileInfo> childrenFiles) =>
            childrenFiles.Select(MapToFileAttributes);

        private static FileMetadata MapToFileAttributes(FileInfo childrenFile)
        {
            return new FileMetadata
            {
                FullName = childrenFile.FullName,
                Name = childrenFile.Name,
                Size = childrenFile.Length
            };
        }
        
        public class DirectoryMetadata
        {
            public string FullName { get; set; }
        }
        
        public class FileMetadata
        {
            public string FullName { get; set; }
            public string Name { get; set; }
            public long Size { get; set; }
        }

       
    }
}