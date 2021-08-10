using System.IO;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Refactored.Parts;

namespace XDataFlow.Parts.FileSystems
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