using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SharpGlide.FilesToCloudUploader.Tunnels;
using SharpGlide.Parts;
using SharpGlide.Tunnels.Readers.Proxy;

namespace SharpGlide.FilesToCloudUploader.Parts
{
    public class CalculateTotalSizePart : IBasePart
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CalculateTotalSizePart> _logger;
        private readonly IWalkerByRequestProxy<FileAttributes, DirectoryRequest> _walkProxy;
        
        public string Name { get; set; }

        public CalculateTotalSizePart(
            IConfiguration configuration,
            ILogger<CalculateTotalSizePart> logger,
            IWalkerByRequestProxy<FileAttributes, DirectoryRequest> walkProxy)
        {
            _configuration = configuration;
            _logger = logger;
            _walkProxy = walkProxy;
        }
        
        public async Task ProcessAsync(CancellationToken cancellationToken)
        {
            var files = new List<FileAttributes>();
            await _walkProxy.WalkAsync(cancellationToken, new DirectoryRequest()
            {
                FullName = _configuration.GetValue<string>("ContextFolder")
            },fileAttr => { files.Add(fileAttr); });
            
            _logger.LogInformation("Done. Total files {fileCount}", files.Count);
            _logger.LogInformation("Total size is {totalSize} bytes", files.Sum(s => s.Size));
        }
    }
}