using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Data.SqlClient;
using XDataFlow.Parts.Abstractions;
using Timer = System.Timers.Timer;

namespace XDataFlow.Parts.Net.MsSql
{

    // public class Buffer<TConsumeData, TPublishData> : VectorPart<TConsumeData, TPublishData>
    // {
    //     private readonly List<TConsumeData> _inputBuffer = new List<TConsumeData>();
    //
    //     private readonly Timer _bufferProcessTimer = new Timer(250);
    //
    //     private readonly int _bufferAmountLimit = 1000;
    //
    //     public Buffer()
    //     {
    //         _bufferProcessTimer.Elapsed += BufferProcessTimerOnElapsed;
    //         _bufferProcessTimer.Start();
    //     }
    //
    //     private void BufferProcessTimerOnElapsed(object sender, ElapsedEventArgs e)
    //     {
    //         throw new System.NotImplementedException();
    //     }
    //
    //     public override Task ProcessAsync(TConsumeData data, CancellationToken cancellationToken)
    //     {
    //         _inputBuffer.Add(data);
    //     }
    // }

    // public class SqlExecutor : VectorPart<SqlExecutor.Input, SqlExecutor.Output>
    // {
    //     private readonly string _connectionString;
    //     
    //     public class Input
    //     {
    //         public string SqlQuery { get; set; }
    //     }
    //     
    //     public class Output
    //     {
    //         public string SqlQuery { get; set; }
    //
    //         public bool Result { get; set; }
    //         
    //         public SqlException SqlException { get; set; }
    //     }
    //
    //     public SqlExecutor(string connectionString)
    //     {
    //         _connectionString = connectionString;
    //         
    //       
    //     }
    //
    //     private void BufferProcessTimerOnElapsed(object sender, ElapsedEventArgs e)
    //     {
    //         if (!_inputBuffer.Any())
    //         {
    //             return;
    //         }
    //         
    //         lock (_inputBuffer)
    //         {
    //             var builder = new SqlConnectionStringBuilder {ConnectionString = _connectionString};
    //             using var connection = new SqlConnection(builder.ConnectionString);
    //
    //             connection.Open();
    //
    //             var command = connection.CreateCommand();
    //             command.Connection = connection;
    //
    //             var processed = new List<Input>();
    //
    //             foreach (var bufferEntry in _inputBuffer)
    //             {
    //                 try
    //                 {
    //                     command.CommandText = bufferEntry.SqlQuery;
    //                     command.ExecuteNonQuery();
    //
    //                     processed.Add(bufferEntry);
    //                 }
    //                 catch (SqlException sqlEx)
    //                 {
    //                     this.Status["Error"] = $"{sqlEx.Message}; query=>{bufferEntry.SqlQuery}";
    //                 }
    //             }
    //
    //             foreach (var processedEntry in processed)
    //             {
    //                 _inputBuffer.Remove(processedEntry);
    //             }
    //         }
    //     }
    //
    //     protected override void ProcessMessage(Input data)
    //     {
    //         lock (_inputBuffer)
    //         {
    //             _inputBuffer.Add(data);
    //         }
    //     }
    //
    //     public override Task ProcessAsync(Input data, CancellationToken cancellationToken)
    //     {
    //         throw new System.NotImplementedException();
    //     }
    // }
}