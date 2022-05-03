using SharpGlide.Cloud.Yandex.Model;
using SharpGlide.Cloud.Yandex.Tunnels.YandexDisk.Model;
using SharpGlide.Writers.Interfaces;

namespace SharpGlide.Cloud.Yandex.Writers.YandexDisc
{
    public interface ISingleFolderCreator:  ISingleWriter<ICloudFolderInformation, IAuthorizeTokens>
    {
        
    }
}