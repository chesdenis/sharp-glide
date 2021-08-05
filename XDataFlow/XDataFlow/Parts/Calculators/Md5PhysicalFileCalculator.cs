using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Extensions;
using XDataFlow.Refactored.Controllers.Consume;
using XDataFlow.Refactored.Controllers.Group;
using XDataFlow.Refactored.Controllers.MetaData;
using XDataFlow.Refactored.Parts;

namespace XDataFlow.Parts.Calculators
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
        
        public override Task ProcessAsync(Input data, CancellationToken cancellationToken)
        {
            data.FilePath = data.FilePath ?? throw new InvalidOperationException();
            
            
            
            this.Status["InProgress"] = Path.GetFileName(data.FilePath);
                
            using var md5 = MD5.Create();
 
            using var stream = System.IO.File.OpenRead(data.FilePath);
            
            var hash = md5.ComputeHash(stream);
            var md5AsString = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

            this.Publish<Md5PhysicalFileCalculator, Output>(new Output()
            {
                FilePath = data.FilePath,
                Md5AsString = md5AsString
            });
        }
    }
}