using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SharpGlide.Context.Abstractions
{
    public interface IConsumeContext<TConsumeData>
    {
        Func<IEnumerable<TConsumeData>> ConsumeDataPointer { get; set; }
        void BuildConsumeLogic(Expression<Func<IEnumerable<TConsumeData>>> logic);
        IEnumerable<TConsumeData> Consume();
    }
}