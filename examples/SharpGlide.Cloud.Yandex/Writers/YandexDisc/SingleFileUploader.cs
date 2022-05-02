using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Cloud.Yandex.Model;
using SharpGlide.Cloud.Yandex.Tunnels.YandexDisk;
using SharpGlide.Cloud.Yandex.Tunnels.YandexDisk.Model;
using SharpGlide.Routing;
using SharpGlide.Writers.Abstractions;

namespace SharpGlide.Cloud.Yandex.Writers.YandexDisc
{
    public class SingleFileUploader :
        SingleWriter<ICloudFileInformation, IAuthorizeTokens>,
        ISingleFileUploader
    {
        public SingleFileUploader(
            Func<IAuthorizeTokens, ICloudFileInformation, IRoute, CancellationToken, Task>
                singleWriteFunc,
            Func<IAuthorizeTokens, ICloudFileInformation, IRoute, CancellationToken,
                Task<ICloudFileInformation>> singleWriteAndReturnFunc) : base(singleWriteFunc,
            singleWriteAndReturnFunc)
        {
        }
    }
}