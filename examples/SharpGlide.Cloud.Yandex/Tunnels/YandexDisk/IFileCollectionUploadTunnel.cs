using SharpGlide.Cloud.Yandex.Model;
using SharpGlide.Cloud.Yandex.Tunnels.YandexDisk.Model;
using SharpGlide.Tunnels.Write.Interfaces;

namespace SharpGlide.Cloud.Yandex.Tunnels.YandexDisk
{
    public interface IFileCollectionUploadTunnel : 
        ICollectionWriteTunnel<ICloudFileInformation, IAuthorizeTokens>
    {
        
    }
}