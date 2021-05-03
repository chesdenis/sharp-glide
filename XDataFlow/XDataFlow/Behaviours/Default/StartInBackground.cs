using System.Threading.Tasks;
using XDataFlow.Parts;

namespace XDataFlow.Behaviours.Default
{
    public class StartInBackground : IStartBehaviour
    {
        public void Execute(IRestartablePart part)
        {
            Task.Run(() => part.StartPointer()());
        }
    }
}