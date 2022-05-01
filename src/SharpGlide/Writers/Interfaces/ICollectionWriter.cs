using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Routing;

namespace SharpGlide.Writers.Interfaces
{
    public interface ICollectionWriter<T>
    {
        Task Write(IEnumerable<T> dataRange, IRoute route, CancellationToken cancellationToken);
        Task<IEnumerable<T>> WriteAndReturn(IEnumerable<T> dataRange, IRoute route, CancellationToken cancellationToken);
    }
    
    public interface ICollectionWriter<T, in TArg>
    {
        Task Write(TArg arg, IEnumerable<T> dataRange, IRoute route, CancellationToken cancellationToken);
        Task<IEnumerable<T>> WriteAndReturn(TArg arg, IEnumerable<T> dataRange, IRoute route, CancellationToken cancellationToken);
    }
}