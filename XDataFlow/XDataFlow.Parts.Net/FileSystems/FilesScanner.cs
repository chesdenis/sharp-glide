using System.IO;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Parts.Abstractions;
using XDataFlow.Registry;

namespace XDataFlow.Parts.Net.FileSystems
{
    public class FilesScanner: VectorPart<FilesScanner.Input, FilesScanner.Output>
    {
        public class Input
        {
            public string InitialPath { get; set; }
        }
        
        public class Output
        {
            public string LeafRef { get; set; }
        }

        public FilesScanner(IDefaultRegistry defaultRegistry) : base(defaultRegistry)
        {
        }

        public override Task ProcessAsync(Input data, CancellationToken cancellationToken)
        {
            var dirInfo = new DirectoryInfo(data.InitialPath);
            
            var files = dirInfo.EnumerateFiles("*", SearchOption.AllDirectories);
            
            foreach (var file in files)
            {
                this.Publish(new Output() {LeafRef = file.FullName});
            }

            return Task.CompletedTask;
        }
    }
}