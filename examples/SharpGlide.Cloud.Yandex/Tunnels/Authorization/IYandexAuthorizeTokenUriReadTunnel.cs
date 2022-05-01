using SharpGlide.Tunnels.Read.Interfaces;

namespace SharpGlide.Cloud.Yandex.Tunnels.Authorization
{
    public interface IYandexAuthorizeTokenUriReadTunnel : ISingleReadTunnel<string, IYandexAuthorizeTokenUriReadTunnel.IAuthTokenRequest>
    {
        public interface IAuthTokenRequest
        {
            string ClientId { get; set; }
            string RedirectUri { get; set; }
        }
    }
}