using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SharpGlide.FilesToCloudUploader.Tunnels;
using SharpGlide.Parts;
using SharpGlide.Readers;
using SharpGlide.Readers.Interfaces;

namespace SharpGlide.FilesToCloudUploader.Parts
{
    public class UploadFileToYandexPart : IBasePart
    {
        private readonly IConfiguration _configuration;
        private readonly ISingleReader<OAuthAuthorizeReadTunnel.OAuthResponse, OAuthAuthorizeReadTunnel.OAuthRequest> _oauthReader;
        public string Name { get; set; }

        public UploadFileToYandexPart(
            IConfiguration configuration,
            ILogger<UploadFileToYandexPart> logger,
            ISingleReader<OAuthAuthorizeReadTunnel.OAuthResponse, OAuthAuthorizeReadTunnel.OAuthRequest> oauthReader)
        {
            _configuration = configuration;
            _oauthReader = oauthReader;
        }
        public async Task ProcessAsync(CancellationToken cancellationToken)
        {
            await _oauthReader.ReadSingleAsync(cancellationToken, new OAuthAuthorizeReadTunnel.OAuthRequest
            {
                ClientId = _configuration["SpYadDiskUploader:OAuth:ClientId"],
                SecretId = _configuration["SpYadDiskUploader:OAuth:SecretId"]
            });
        }
    }
}