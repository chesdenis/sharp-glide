using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Readers.Interfaces;
using SharpGlide.Tunnels.Readers.Model;

namespace SharpGlide.Tunnels.Readers.Abstractions
{
    public abstract class DirectWalker<T> : IDirectWalker<T>
    {
        public bool CanExecute { get; set; }


        public Expression<Func<CancellationToken,Action<T>, Task>> WalkExpr => 
            (cancellationToken, callBack) => WalkImpl(cancellationToken, callBack);
        protected abstract Task WalkImpl(CancellationToken cancellationToken, Action<T> callBack);

        public Expression<Func<CancellationToken, PageInfo, Action<IEnumerable<T>>, Task>> WalkPagedExpr =>
            (cancellationToken, pageInfo, callback) => WalkPagedImpl(cancellationToken, pageInfo, callback);

        protected abstract Task WalkPagedImpl(CancellationToken cancellationToken, PageInfo pageInfo, Action<IEnumerable<T>> callback);
    }
}