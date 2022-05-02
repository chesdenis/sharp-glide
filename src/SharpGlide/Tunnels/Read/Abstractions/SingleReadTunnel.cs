using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SharpGlide.Tunnels.Read.Interfaces;

namespace SharpGlide.Tunnels.Read.Abstractions
{
    public abstract class SingleReadTunnel<T> : ISingleReadTunnel<T>
    {
        public bool CanExecute { get; set; }

        public Expression<Func<CancellationToken, Task<T>>> ReadSingleExpr =>
            (cancellationToken)
                => SingleReadImpl(cancellationToken);
        
        protected abstract Task<T> SingleReadImpl(CancellationToken cancellationToken);
    }
    
    public abstract class SingleReadTunnel<T, TArg> : ISingleReadTunnel<T, TArg>
    {
        public bool CanExecute { get; set; }

        public Expression<Func<CancellationToken,TArg, Task<T>>> ReadSingleExpr =>
            (cancellationToken, arg)
                => SingleReadImpl(cancellationToken, arg);
        
        protected abstract Task<T> SingleReadImpl(CancellationToken cancellationToken, TArg arg);
    }
}