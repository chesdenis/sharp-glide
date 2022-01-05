using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpGlide.Context.Abstractions;

namespace SharpGlide.Context
{
    public class ConsumeContext<TConsumeData> : IConsumeContext<TConsumeData>
    {
        public Func<IEnumerable<TConsumeData>> ConsumeDataPointer { get; set; }

        public void SetConsumeFlow(Expression<Func<IEnumerable<TConsumeData>>> flowExpr)
        {
            ConsumeDataPointer = flowExpr
                .Compile();
        }

        public IEnumerable<TConsumeData> Consume() => ConsumeDataPointer();
    }
}