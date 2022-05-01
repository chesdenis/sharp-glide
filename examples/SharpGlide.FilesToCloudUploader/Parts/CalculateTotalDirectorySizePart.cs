using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SharpGlide.FilesToCloudUploader.Tunnels;
using SharpGlide.Parts;
using SharpGlide.Readers;

namespace SharpGlide.FilesToCloudUploader.Parts
{
    public class CalculateTotalDirectorySizePart : IBasePart
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CalculateTotalDirectorySizePart> _logger;
        private readonly ISingleWalker<FolderContentsWalkTunnel.FileMetadata, FolderContentsWalkTunnel.DirectoryMetadata> _walkProxy;
        
        public string Name { get; set; }

        public CalculateTotalDirectorySizePart(
            IConfiguration configuration,
            ILogger<CalculateTotalDirectorySizePart> logger,
            ISingleWalker<FolderContentsWalkTunnel.FileMetadata, FolderContentsWalkTunnel.DirectoryMetadata> walkProxy)
        {
            _configuration = configuration;
            _logger = logger;
            _walkProxy = walkProxy;
        }
        
        public async Task ProcessAsync(CancellationToken cancellationToken)
        {
            var files = new List<FolderContentsWalkTunnel.FileMetadata>();
            await _walkProxy.WalkAsync(cancellationToken, new FolderContentsWalkTunnel.DirectoryMetadata()
            {
                FullName = _configuration.GetValue<string>("ContextFolder")
            },fileAttr => { files.Add(fileAttr); });
            
            _logger.LogInformation("Done. Total files {fileCount}", files.Count);
            _logger.LogInformation("Total size is {totalSize} bytes", files.Sum(s => s.Size));
        }
    }
}