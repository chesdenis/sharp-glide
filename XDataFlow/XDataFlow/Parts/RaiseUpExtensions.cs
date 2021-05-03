namespace XDataFlow.Parts
{
    public static class FlowPartExtensions
    {
        public static void Start<TDataFlowPart>(this TDataFlowPart flowPart) where TDataFlowPart : IRestartablePart
        {
            foreach (var onRaiseUp in flowPart.StartBehaviours) onRaiseUp.Execute(flowPart);
        }
    }
}