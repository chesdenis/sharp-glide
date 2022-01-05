using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpGlide.Tunnels.Routes.XRoutes;

namespace SharpGlide.Context.Abstractions
{
    public interface IConsumeContext<TConsumeData>
    {
        Func<IEnumerable<TConsumeData>> ConsumeDataPointer { get; set; }
        void SetConsumeFlow(
            Expression<Func<IEnumerable<TConsumeData>, 
                IXConsumeRoute>> flowExpr);
        IEnumerable<TConsumeData> Consume();
    }
}