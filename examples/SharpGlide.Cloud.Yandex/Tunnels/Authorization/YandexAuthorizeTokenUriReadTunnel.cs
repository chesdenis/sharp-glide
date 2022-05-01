using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Abstractions;

namespace SharpGlide.Cloud.Yandex.Tunnels.Authorization
{
    public class YandexAuthorizeTokenUriReadTunnel : 
        SingleReadTunnel<string, IYandexAuthorizeTokenUriReadTunnel.IAuthTokenRequest>
    {
        private const string HostUri = "https://oauth.yandex.ru/authorize";
        protected override Task<string> SingleReadImpl(CancellationToken cancellationToken, IYandexAuthorizeTokenUriReadTunnel.IAuthTokenRequest arg)
        {
            return Task.FromResult(
                $"{HostUri}?response_type=code&client_id={arg.ClientId}&redirect_uri={arg.RedirectUri}");
        }
    }
}