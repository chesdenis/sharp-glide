using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Readers.Interfaces;
using SharpGlide.Tunnels.Readers.Model;

namespace SharpGlide.Tunnels.Readers.Abstractions
{
    public abstract class ReaderByRequest<T, TRequest> : IReaderByRequest<T, TRequest>
    {
        public bool CanExecute { get; set; }
        public virtual Expression<Func<CancellationToken, TRequest, Task<T>>> ReadExpr => 
            (cancellationToken, request) 
                => ReadSingleImpl(cancellationToken, request);
        protected abstract Task<T> ReadSingleImpl(CancellationToken cancellationToken, TRequest request);

        public virtual Expression<Func<CancellationToken, TRequest, Task<IEnumerable<T>>>> ReadAllExpr => (cancellationToken, request) 
            => ReadAllImpl(cancellationToken, request);
        protected abstract Task<IEnumerable<T>> ReadAllImpl(CancellationToken cancellationToken, TRequest request);
        
        public virtual Expression<Func<CancellationToken, TRequest, Func<Task<IEnumerable<T>>, PageInfo>, PageInfo>> ReadPagedExpr=>
            (cancellationToken, request, onPageRead) => 
                ReadPagedImpl(cancellationToken, request, onPageRead);
        protected abstract PageInfo ReadPagedImpl(CancellationToken cancellationToken, TRequest request,
            Func<Task<IEnumerable<T>>, PageInfo> onPageRead);

        public virtual
            Expression<Func<CancellationToken, TRequest, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>>>
            ReadSpecificExpr
            => (cancellationToken, request, filter) => ReadSpecificImpl(cancellationToken, request, filter);
        
        protected abstract Task<IEnumerable<T>> ReadSpecificImpl(
            CancellationToken cancellationToken,
            TRequest request,
            Func<IEnumerable<T>, IEnumerable<T>> filter);
    }
}