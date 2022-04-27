using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharpGlide.Extensions;
using SharpGlide.Tunnels.Readers.Abstractions;
using SharpGlide.Tunnels.Readers.Model;

namespace SharpGlide.FilesToCloudUploader.Tunnels
{
    public class FolderContentsWalker : WalkerByRequest<FileAttributes, DirectoryRequest>
    {
        private readonly ILogger<FolderContentsWalker> _logger;

        public FolderContentsWalker(ILogger<FolderContentsWalker> logger)
        {
            _logger = logger;
        }

        protected override async Task WalkExprImpl(
            CancellationToken cancellationToken,
            DirectoryRequest request,
            Action<FileAttributes> callback)
        {
            await WalkDirectory(request, callback, null);
        }

        protected override async Task WalkRangeExprImpl(CancellationToken cancellationToken,
            DirectoryRequest request,
            Action<IEnumerable<FileAttributes>> callbackRange)
        {
            await WalkDirectory(request, null, callbackRange);
        }

        protected override Task WalkPagedImpl(
            CancellationToken cancellationToken,
            PageInfo pageInfo,
            DirectoryRequest request,
            Action<IEnumerable<FileAttributes>> callback)
        {
            throw new NotImplementedException();
        }

        private async Task WalkDirectory(DirectoryRequest request,
            Action<FileAttributes> callbackSingle,
            Action<FileAttributes[]> callbackRange)
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
                    await WalkDirectory(new DirectoryRequest()
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

        private void WalkFiles(Action<FileAttributes> callbackSingle, FileInfo[] childrenFiles, DirectoryInfo directory)
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

        private static IEnumerable<FileAttributes> MapToFileAttributes(IEnumerable<FileInfo> childrenFiles) =>
            childrenFiles.Select(MapToFileAttributes);

        private static FileAttributes MapToFileAttributes(FileInfo childrenFile)
        {
            return new FileAttributes
            {
                FullName = childrenFile.FullName,
                Name = childrenFile.Name,
                Size = childrenFile.Length
            };
        }
    }
}