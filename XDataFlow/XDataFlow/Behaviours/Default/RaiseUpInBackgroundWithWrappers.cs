using System.Threading.Tasks;
using XDataFlow.Parts;

namespace XDataFlow.Behaviours.Default
{
    public class RaiseUpInBackgroundWithWrappers : IRaiseUpBehaviour
    {
        public void Execute(IDataFlowPart part)
        {
            var entryPointer = part.EntryPointer();

            foreach (var wrapper in part.OnEntryWrappers)
            {
                entryPointer = wrapper.Wrap(entryPointer);
            }

            Task.Run(() => entryPointer());
        }
    }
}