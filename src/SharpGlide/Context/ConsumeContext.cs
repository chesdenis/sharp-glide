using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SharpGlide.Context.Abstractions;

namespace SharpGlide.Context
{
    public class ConsumeContext<TConsumeData> : IConsumeContext<TConsumeData>
    {
        public Func<IEnumerable<TConsumeData>> ConsumeDataPointer { get; set; }

        public void BuildConsumeLogic(Expression<Func<IEnumerable<TConsumeData>>> logic)
        {
            ConsumeDataPointer = logic
                .Compile();
        }

        public IEnumerable<TConsumeData> Consume() => ConsumeDataPointer();
    }
}