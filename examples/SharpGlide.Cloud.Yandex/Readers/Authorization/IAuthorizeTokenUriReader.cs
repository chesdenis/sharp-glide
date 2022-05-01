using SharpGlide.Cloud.Yandex.Tunnels.Authorization;
using SharpGlide.Readers.Interfaces;

namespace SharpGlide.Cloud.Yandex.Readers.Authorization
{
    public interface IAuthorizeTokenUriReader : 
        ISingleReader<string, IAuthorizeTokenUriReadTunnel.IAuthTokenRequest>
    {
        
    }
}