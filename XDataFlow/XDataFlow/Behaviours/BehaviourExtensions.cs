using XDataFlow.Parts;

namespace XDataFlow.Behaviours
{
    public static class BehaviourExtensions
    {
        public static void AddStartBehaviour<TBehaviour>(this IPart part)
                where TBehaviour : IStartBehaviour, new()
        {
            part.StartBehaviours.Add(new TBehaviour());
        }
    }
}