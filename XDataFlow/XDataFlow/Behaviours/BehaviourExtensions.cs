using XDataFlow.Parts;

namespace XDataFlow.Behaviours
{
    public static class BehaviourExtensions
    {
        public static void AddStartBehaviour<TBehaviour>(this IRestartablePart part)
                where TBehaviour : IStartBehaviour, new()
        {
            part.StartBehaviours.Add(new TBehaviour());
        }
    }
}