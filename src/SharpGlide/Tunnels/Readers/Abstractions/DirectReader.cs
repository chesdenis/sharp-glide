using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Readers.Interfaces;
using SharpGlide.Tunnels.Readers.Model;

namespace SharpGlide.Tunnels.Readers.Abstractions
{
    public abstract class DirectReader<T> : IDirectReader<T>
    {
        public bool CanExecute { get; set; }
        public virtual Expression<Func<CancellationToken, Task<T>>> ReadExpr => 
            (cancellationToken) 
                => ReadSingleImpl(cancellationToken);
        protected abstract Task<T> ReadSingleImpl(CancellationToken cancellationToken);

        public virtual Expression<Func<CancellationToken, Task<IEnumerable<T>>>> ReadAllExpr =>
            (cancellationToken) 
                => ReadAllImpl(cancellationToken);
        protected abstract Task<IEnumerable<T>> ReadAllImpl(CancellationToken cancellationToken);

        public Expression<Func<CancellationToken, PageInfo, Task<IEnumerable<T>>>> ReadPagedExpr =>
            ((cancellationToken, pageInfo) => ReadPagedImpl(cancellationToken, pageInfo));

        protected abstract Task<IEnumerable<T>> ReadPagedImpl(CancellationToken cancellationToken, PageInfo pageInfo);
        
        public virtual Expression<Func<CancellationToken, Func<IEnumerable<T>, IEnumerable<T>>, Task<IEnumerable<T>>>>
            ReadSpecificExpr => (cancellationToken, filter) => ReadSpecificImpl(cancellationToken, filter);

        protected abstract Task<IEnumerable<T>> ReadSpecificImpl(
            CancellationToken cancellationToken,
            Func<IEnumerable<T>, IEnumerable<T>> filter);
 
    }
}