using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XDataFlow.Parts.Generic;
using XDataFlow.Registry;

namespace XDataFlow.Parts.Net.Calculators
{
    public class Md5StringDecorator<TDecorateData> : GenericDecorator<TDecorateData, string, string>
    {
        public Md5StringDecorator(IDefaultRegistry defaultRegistry) : base(defaultRegistry)
        {
        }

        public override Task ProcessAsync(TDecorateData data, CancellationToken cancellationToken)
        {
            // Use input string to calculate MD5 hash
            using var md5 = System.Security.Cryptography.MD5.Create();

            var inputBytes = System.Text.Encoding.UTF8.GetBytes(InputPointer());
            var hashBytes = md5.ComputeHash(inputBytes);
    
            // Convert the byte array to hexadecimal string
            var sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            
            AssignActionPointer(sb.ToString());

            this.Publish(data);

            return Task.CompletedTask;
        }
    }
}