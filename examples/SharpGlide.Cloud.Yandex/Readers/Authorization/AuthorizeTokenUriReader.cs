using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Cloud.Yandex.Tunnels.Authorization;
using SharpGlide.Readers.Abstractions;

namespace SharpGlide.Cloud.Yandex.Readers.Authorization
{
    public class AuthorizeTokenUriReader :
        SingleReader<string, IAuthorizeTokenUriReadTunnel.IAuthTokenRequest>, IAuthorizeTokenUriReader
    {
        public AuthorizeTokenUriReader(
            Func<CancellationToken, IAuthorizeTokenUriReadTunnel.IAuthTokenRequest, Task<string>> 
                singleReadFunc) : base(singleReadFunc)
        {
        }
    }
}