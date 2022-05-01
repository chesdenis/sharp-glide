using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Abstractions;
using SharpGlide.Tunnels.Read.Interfaces;

namespace SharpGlide.Cloud.Yandex.Tunnels.Authorization
{
    public class AuthorizeTokenUriReadTunnel :
        SingleReadTunnel<string, IAuthorizeTokenUriReadTunnel.IAuthTokenRequest>,
        IAuthorizeTokenUriReadTunnel
    {
        private const string HostUri = "https://oauth.yandex.ru/authorize";

        protected override Task<string> SingleReadImpl(CancellationToken cancellationToken,
            IAuthorizeTokenUriReadTunnel.IAuthTokenRequest arg)
        {
            return Task.FromResult(
                $"{HostUri}?response_type=token&client_id={arg.ClientId}&redirect_uri={arg.RedirectUri}");
        }
    }
}