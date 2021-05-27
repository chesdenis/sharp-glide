using System.IO;

namespace XDataFlow.Parts.FileSystems
{
    public class FilesScanner: FlowPart<FilesScanner.Input, FilesScanner.Output>
    {
        public class Input
        {
            public string InitialPath { get; set; }
        }
        
        public class Output
        {
            public string LeafRef { get; set; }
        }
          
        protected override void ProcessMessage(Input data)
        {
            var dirInfo = new DirectoryInfo(data.InitialPath);
            
            var files = dirInfo.EnumerateFiles("*", SearchOption.AllDirectories);
            
            foreach (var file in files)
            {
                this.Publish(new Output() {LeafRef = file.FullName});
            }
        }
    }
}