using System.Threading.Tasks;
using XDataFlow.Parts;

namespace XDataFlow.Behaviours.Default
{
    public class StartInBackground : IStartBehaviour
    {
        public void Execute(IPart part)
        {
            Task.Run(() => part.StartPointer()());
        }
    }
}