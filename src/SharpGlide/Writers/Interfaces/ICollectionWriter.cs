using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;

namespace SharpGlide.Writers.Interfaces
{
    public interface ICollectionWriter<T>
    {
        Task WriteCollection(IEnumerable<T> dataRange, IRoute route, CancellationToken cancellationToken);
        Task<IEnumerable<T>> WriteAndReturnCollection(IEnumerable<T> dataRange, IRoute route, CancellationToken cancellationToken);
    }
    
    public interface ICollectionWriter<T, in TArg>
    {
        Task WriteCollection(TArg arg, IEnumerable<T> dataRange, IRoute route, CancellationToken cancellationToken);
        Task<IEnumerable<T>> WriteAndReturnCollection(TArg arg, IEnumerable<T> dataRange, IRoute route, CancellationToken cancellationToken);
    }
}