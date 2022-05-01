using SharpGlide.Cloud.Yandex.SharedModel;
using SharpGlide.Cloud.Yandex.Tunnels.Profile.Model;
using SharpGlide.Tunnels.Read.Interfaces;

namespace SharpGlide.Cloud.Yandex.Tunnels.Profile
{
    public interface IProfileReadTunnel : ISingleReadTunnel<ProfileResponse, AuthorizeTokens>
    {
        
    }
}