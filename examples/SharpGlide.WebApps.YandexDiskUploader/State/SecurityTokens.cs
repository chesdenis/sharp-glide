using SharpGlide.Cloud.Yandex.Model;

namespace SharpGlide.WebApps.YandexDiskUploader.State
{
    public class SecurityTokens : IAuthorizeTokens
    {
        public string AccessToken { get; set; }
    }
}