using System.Threading;
using System.Threading.Tasks;

namespace SharpGlide.Parts
{
    public interface IBasePart
    {
        string Name { get; set; }
        public Task ProcessAsync(CancellationToken cancellationToken);
    }
}