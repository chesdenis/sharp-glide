using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.IO.Model;
using SharpGlide.Tunnels.Read.Abstractions;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.IO.Tunnels
{
    public class FileContentWalkTunnel : PagedWalkTunnel<byte, IFileBytesRangeRequest>, IFileContentWalkTunnel
    {
        protected override async Task PagedWalkImpl(CancellationToken cancellationToken,
            PageInfo pageInfo, IFileBytesRangeRequest request, Action<IEnumerable<byte>> callback)
        {
            await WalkFileBytes(request, pageInfo, null, callback, cancellationToken);
        }

        protected override async Task PagedAsyncWalkImpl(CancellationToken cancellationToken,
            PageInfo pageInfo, IFileBytesRangeRequest request,
            Func<CancellationToken, IEnumerable<byte>, Task> callback)
        {
            await WalkFileBytes(request, pageInfo, callback, null, cancellationToken);
        }

        private static async Task WalkFileBytes(IFileBytesRangeRequest request,
            PageInfo pageInfo,
            Func<CancellationToken, IEnumerable<byte>, Task> asyncCallback,
            Action<IEnumerable<byte>> callback,
            CancellationToken cancellationToken)
        {
            var fileStream = File.OpenRead(request.FullName);
            var buffer = new byte[pageInfo.PageSize];

            try
            {
                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }
                
                    var position = await fileStream.ReadAsync(buffer,
                        pageInfo.PageIndex * pageInfo.PageSize, 
                        pageInfo.PageSize, cancellationToken);

                    if (asyncCallback != null)
                    {
                        await asyncCallback(cancellationToken, buffer);
                    }
                
                    callback?.Invoke(buffer);

                    if (position <= 0)
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Unable to read files bytes for {request.FullName}", e);
            }
        }
    }
}