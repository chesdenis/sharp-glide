using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SharpGlide.Routing;
using SharpGlide.Tunnels.Writers;

namespace SharpGlide.Tests.Behaviour.Tunnels.Concept.Examples
{
    public class DirectWriterExample : IDirectWriter<decimal>
    {
        public readonly ConcurrentStack<decimal> Stack = new();
    
        public bool CanExecute { get; set; } = true;
        public Expression<Action<decimal, IRoute>> WriteExpr => (v, rt) => WriteLogic(v,rt);
        public Expression<Action<IEnumerable<decimal>, IRoute>> WriteRangeExpr => (vs, rt) => WriteRangeLogic(vs, rt);
        
        private void WriteLogic(decimal v, IRoute rt)
        {
            Stack.Push(v);
        }
            
        private void WriteRangeLogic(IEnumerable<decimal> vs, IRoute rt)
        {
            Stack.PushRange(vs.ToArray());
        }
    }
}