using SharpGlide.Cloud.Yandex.Model;
using SharpGlide.Cloud.Yandex.Tunnels.Profile.Model;
using SharpGlide.Readers.Interfaces;

namespace SharpGlide.Cloud.Yandex.Readers.Profile
{
    public interface IProfileReader : ISingleReader<ProfileResponse, AuthorizeTokens>
    {
        
    }
}