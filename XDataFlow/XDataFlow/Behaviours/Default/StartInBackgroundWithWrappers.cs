using System.Threading.Tasks;
using XDataFlow.Parts;

namespace XDataFlow.Behaviours.Default
{
    public class StartInBackgroundWithWrappers : IStartBehaviour
    {
        public void Execute(IPart part)
        {
            var entryPointer = part.StartPointer();

            foreach (var wrapper in part.StartWrappers)
            {
                entryPointer = wrapper.Wrap(entryPointer);
            }

            Task.Run(() => entryPointer());
        }
    }
}