using System;
using System.Text;
using XDataFlow.Refactored.Parts;

namespace XDataFlow.Parts.Calculators
{
    // TODO: Implement md5 string decorator
    // public class Md5StringDecorator<TDecorateData> : VectorPart<TDecorateData, TDecorateData>
    // {
    //     private readonly Func<TDecorateData, string> _inputPointer;
    //     private readonly Action<string, TDecorateData> _assignActionPointer;
    //     
    //     public Md5StringDecorator(
    //         Func<TDecorateData, string> inputPointer,
    //         Action<string, TDecorateData> assignActionPointer)
    //     {
    //         _inputPointer = inputPointer;
    //         _assignActionPointer = assignActionPointer;
    //     }
    //     
    //     protected override void ProcessMessage(TDecorateData data)
    //     {
    //         // Use input string to calculate MD5 hash
    //         using var md5 = System.Security.Cryptography.MD5.Create();
    //         var inputBytes = System.Text.Encoding.UTF8.GetBytes(_inputPointer(data));
    //         var hashBytes = md5.ComputeHash(inputBytes);
    //
    //         // Convert the byte array to hexadecimal string
    //         var sb = new StringBuilder();
    //         for (int i = 0; i < hashBytes.Length; i++)
    //         {
    //             sb.Append(hashBytes[i].ToString("X2"));
    //         }
    //
    //         _assignActionPointer(sb.ToString(), data);
    //         
    //         this.Publish(data);
    //     }
    // }
}