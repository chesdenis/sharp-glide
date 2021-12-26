using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpGlide.Context.Abstractions;
using SharpGlide.Tunnels.Abstractions;
using SharpGlide.Tunnels.Routes;

namespace SharpGlide.Context
{
    public class ConsumeContext<TConsumeData> : IConsumeContext<TConsumeData>
    {
        public BlockExpression ConsumeDataTunnelExpression { get; set; }

        public Func<IEnumerable<TConsumeData>> ConsumeDataTunnelFunc { get; set; }

        public void Rebuild()
        {
            ConsumeDataTunnelFunc =
                Expression.Lambda<Func<IEnumerable<TConsumeData>>>(ConsumeDataTunnelExpression).Compile();
        }
        
        // TODO: make it async
        public IEnumerable<TConsumeData> Consume() => ConsumeDataTunnelFunc();
    }
}