using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Cloud.Yandex.Model;
using SharpGlide.Cloud.Yandex.Tunnels.YandexDisk;
using SharpGlide.Cloud.Yandex.Tunnels.YandexDisk.Model;
using SharpGlide.Routing;
using SharpGlide.Writers.Abstractions;

namespace SharpGlide.Cloud.Yandex.Writers.YandexDisc
{
    public class FileCollectionUploader :
        CollectionWriter<ICloudFileInformation, IAuthorizeTokens>,
        IFileCollectionUploader
    {
        public FileCollectionUploader(
            Func<IAuthorizeTokens, IEnumerable<ICloudFileInformation>, IRoute, CancellationToken,
                    Task>
                collectionWriteFunc,
            Func<IAuthorizeTokens, IEnumerable<ICloudFileInformation>, IRoute, CancellationToken,
                Task<IEnumerable<ICloudFileInformation>>> collectionWriteAndReturnFunc) : base(
            collectionWriteFunc, collectionWriteAndReturnFunc)
        {
        }
    }
}