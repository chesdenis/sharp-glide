using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Microsoft.Data.SqlClient;

namespace XDataFlow.Parts.Net.MsSql
{
    public class SqlExecutor : FlowPart<SqlExecutor.Input, SqlExecutor.Output>
    {
        private readonly string _connectionString;

        private readonly List<Input> _messageBuffer = new List<Input>();

        private readonly Timer _timer = new Timer(250);

        public class Input
        {
            public string SqlQuery { get; set; }
        }
        
        public class Output
        {
            public string SqlQuery { get; set; }

            public bool Result { get; set; }
            
            public SqlException SqlException { get; set; }
        }

        public SqlExecutor(string connectionString)
        {
            _connectionString = connectionString;
            
            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (!_messageBuffer.Any())
            {
                return;
            }
            
            lock (_messageBuffer)
            {
                var builder = new SqlConnectionStringBuilder {ConnectionString = _connectionString};
                using var connection = new SqlConnection(builder.ConnectionString);

                connection.Open();

                var command = connection.CreateCommand();
                command.Connection = connection;

                var processed = new List<Input>();

                foreach (var bufferEntry in _messageBuffer)
                {
                    try
                    {
                        command.CommandText = bufferEntry.SqlQuery;
                        command.ExecuteNonQuery();

                        processed.Add(bufferEntry);
                    }
                    catch (SqlException sqlEx)
                    {
                        this.Status["Error"] = $"{sqlEx.Message}; query=>{bufferEntry.SqlQuery}";
                    }
                }

                foreach (var processedEntry in processed)
                {
                    _messageBuffer.Remove(processedEntry);
                }
            }
        }

        protected override void ProcessMessage(Input data)
        {
            lock (_messageBuffer)
            {
                _messageBuffer.Add(data);
            }
        }
    }
}