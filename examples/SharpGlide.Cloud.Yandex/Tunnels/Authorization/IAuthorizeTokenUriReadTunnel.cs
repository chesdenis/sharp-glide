using SharpGlide.Tunnels.Read.Interfaces;

namespace SharpGlide.Cloud.Yandex.Tunnels.Authorization
{
    public interface IAuthorizeTokenUriReadTunnel : ISingleReadTunnel<string, IAuthorizeTokenUriReadTunnel.IAuthTokenRequest>
    {
        public interface IAuthTokenRequest
        {
            string ClientId { get; }
            string RedirectUri { get; }
        }
    }
}