using SharpGlide.Readers;
using SharpGlide.Tunnels.Read.Abstractions;

namespace SharpGlide.Cloud.Yandex
{
    public interface IAccessToken
    {
        string AccessToken { get; set; }
        string ExpiresInSec { get; set; }
        string TokenType { get; set; }
    }
}