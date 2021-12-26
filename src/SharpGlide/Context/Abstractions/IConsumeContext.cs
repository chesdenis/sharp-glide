using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SharpGlide.Context.Abstractions
{
    public interface IConsumeContext<TConsumeData>
    {
        BlockExpression ConsumeDataTunnelExpression { get; set; }
        Func<IEnumerable<TConsumeData>> ConsumeDataTunnelFunc { get; set; }
        void Rebuild();
        IEnumerable<TConsumeData> Consume();
    }
}