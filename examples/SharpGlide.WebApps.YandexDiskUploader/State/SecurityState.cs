using SharpGlide.Cloud.Yandex.Model;

namespace SharpGlide.WebApps.YandexDiskUploader.State
{
    public class SecurityState : IAuthorizeTokens
    {
        public string AccessToken { get; set; }
    }
}