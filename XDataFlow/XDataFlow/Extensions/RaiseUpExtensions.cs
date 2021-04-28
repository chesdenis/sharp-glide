using XDataFlow.Behaviours;
using XDataFlow.Parts;

namespace XDataFlow.Extensions
{
    public static class RaiseUpExtensions
    {
        public static void RegisterRaiseUpBehaviour<TBehaviour>(this IDataFlowPart part)
            where TBehaviour : IRaiseUpBehaviour, new()
        {
            part.OnRaiseUp.Add(new TBehaviour());
        }
        
        public static void Start<TDataFlowPart>(this TDataFlowPart flowPart) where TDataFlowPart : IDataFlowPart
        {
            foreach (var onRaiseUp in flowPart.OnRaiseUp) onRaiseUp.Execute(flowPart);
        }
    }
}