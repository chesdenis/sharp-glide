namespace XDataFlow.Parts
{
    public static class FlowPartExtensions
    {
        public static void Start<TDataFlowPart>(this TDataFlowPart flowPart) where TDataFlowPart : IPart
        {
            foreach (var behaviour in flowPart.StartBehaviours) behaviour.Execute(flowPart);
        }
    }
}