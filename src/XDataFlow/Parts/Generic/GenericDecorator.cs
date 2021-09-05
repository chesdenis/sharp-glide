using System;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Parts.Abstractions;
using XDataFlow.Registry;

namespace XDataFlow.Parts.Generic
{
    public class GenericDecorator<TDecorateData, TInput, TOutput> : VectorPart<TDecorateData, TDecorateData>
    {
        public GenericDecorator(IDefaultRegistry defaultRegistry) : base(defaultRegistry)
        {
        }

        public Func<TInput> InputPointer { get; set; }
        public Action<TOutput> AssignActionPointer { get; set; }
        public Action<Func<TInput>, Action<TOutput>> DecorateActions { get; set; }

        public override Task ProcessAsync(TDecorateData data, CancellationToken cancellationToken)
        {
            DecorateActions(InputPointer, AssignActionPointer);
            
            this.Publish(data);

            return Task.CompletedTask;
        }
    }
}