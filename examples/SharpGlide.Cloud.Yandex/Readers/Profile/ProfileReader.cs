using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Cloud.Yandex.Model;
using SharpGlide.Cloud.Yandex.Tunnels.Profile.Model;
using SharpGlide.Readers.Abstractions;

namespace SharpGlide.Cloud.Yandex.Readers.Profile
{
    public class ProfileReader : SingleReader<ProfileResponse, AuthorizeTokens>, IProfileReader
    {
        public ProfileReader(Func<CancellationToken, AuthorizeTokens, Task<ProfileResponse>> singleReadFunc) : base(singleReadFunc)
        {
        }
    }
}