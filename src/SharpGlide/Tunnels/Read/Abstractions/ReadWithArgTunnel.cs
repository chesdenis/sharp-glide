using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Interfaces;
using SharpGlide.Tunnels.Read.Model;

namespace SharpGlide.Tunnels.Read.Abstractions
{
    public abstract class ReadWithArgTunnel<T, TArg> : IReadWithArgTunnel<T, TArg>
    {
        public bool CanExecute { get; set; }
        public virtual Expression<Func<CancellationToken, TArg, Task<T>>> ReadExpr => 
            (cancellationToken, request) 
                => ReadSingleImpl(cancellationToken, request);
        protected abstract Task<T> ReadSingleImpl(CancellationToken cancellationToken, TArg request);

        public virtual Expression<Func<CancellationToken, TArg, Task<IEnumerable<T>>>> ReadAllExpr => (cancellationToken, request) 
            => ReadAllImpl(cancellationToken, request);
        protected abstract Task<IEnumerable<T>> ReadAllImpl(CancellationToken cancellationToken, TArg request);
        
        public virtual Expression<Func<CancellationToken, PageInfo, TArg, Task<IEnumerable<T>>>> ReadPagedExpr=>
            (cancellationToken, pageInfo, request) => 
                ReadPagedImpl(cancellationToken, pageInfo, request);
        protected abstract Task<IEnumerable<T>> ReadPagedImpl(CancellationToken cancellationToken, PageInfo pageInfo, TArg request);

        public virtual
            Expression<Func<CancellationToken, TArg, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>>>
            ReadSpecificExpr
            => (cancellationToken, request, filter) => ReadSpecificImpl(cancellationToken, request, filter);
        
        protected abstract Task<IEnumerable<T>> ReadSpecificImpl(
            CancellationToken cancellationToken,
            TArg request,
            Func<IEnumerable<T>, IEnumerable<T>> filter);
    }
}