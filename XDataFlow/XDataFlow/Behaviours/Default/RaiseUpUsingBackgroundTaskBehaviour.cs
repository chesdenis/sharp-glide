using System.Threading.Tasks;
using XDataFlow.Parts;

namespace XDataFlow.Behaviours.Default
{
    public class RaiseUpUsingBackgroundTaskBehaviour : IRaiseUpBehaviour
    {
        public void Execute(IDataFlowPart part)
        {
            Task.Run(() => part.EntryPointer()());
        }
    }
}