using System;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Cloud.Yandex.Model;
using SharpGlide.Cloud.Yandex.Tunnels.YandexDisk.Model;
using SharpGlide.Routing;
using SharpGlide.Writers.Abstractions;

namespace SharpGlide.Cloud.Yandex.Writers.YandexDisc
{
    public class SingleFolderCreator : SingleWriter<ICloudFolderInformation, IAuthorizeTokens>, ISingleFolderCreator
    {
        public SingleFolderCreator(Func<IAuthorizeTokens, ICloudFolderInformation, IRoute, CancellationToken, Task> singleWriteFunc, Func<IAuthorizeTokens, ICloudFolderInformation, IRoute, CancellationToken, Task<ICloudFolderInformation>> singleWriteAndReturnFunc) : base(singleWriteFunc, singleWriteAndReturnFunc)
        {
        }
    }
}