using Microsoft.Extensions.Configuration;
using SharpGlide.Cloud.Yandex.Tunnels.Authorization;

namespace SharpGlide.WebApps.YandexDiskUploader.Config
{
    public class AppSetting : IAuthorizeTokenUriReadTunnel.IAuthTokenRequest
    {
        private readonly IConfiguration _configuration;

        public AppSetting(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ClientId => _configuration.GetValue<string>("SpYadDiskUploader:OAuth:ClientId");
        public string SecretId => _configuration.GetValue<string>("SpYadDiskUploader:OAuth:SecretId");
        public string RedirectUri => _configuration.GetValue<string>("SpYadDiskUploader:OAuth:RedirectUri");

        public static string WorkingFolder { get; set; }
    }
}