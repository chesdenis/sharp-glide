using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Parts.Abstractions;
using XDataFlow.Registry;

namespace XDataFlow.Parts.Net.Calculators
{
    public class Md5PhysicalFileCalculator :
        VectorPart<Md5PhysicalFileCalculator.Input, Md5PhysicalFileCalculator.Output>
    {
        public class Input
        {
            public string FilePath { get; set; }
        }
        
        public class Output
        {
            public string FilePath { get; set; }

            public string Md5AsString { get; set; }
        }

        public Md5PhysicalFileCalculator(IDefaultRegistry defaultRegistry) : base(defaultRegistry)
        {
        }

        public override Task ProcessAsync(Input data, CancellationToken cancellationToken)
        {
            data.FilePath = data.FilePath ?? throw new InvalidOperationException();

            this.ReportInfo(Path.GetFileName(data.FilePath));
                
            using var md5 = MD5.Create();
 
            using var stream = System.IO.File.OpenRead(data.FilePath);
            
            var hash = md5.ComputeHash(stream);
            var md5AsString = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

            this.Publish(new Output()
            {
                FilePath = data.FilePath,
                Md5AsString = md5AsString
            });

            return Task.CompletedTask;
        }
    }
}