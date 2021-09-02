using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using XDataFlow.Parts.Abstractions;
using XDataFlow.Registry;

namespace XDataFlow.Parts.Net.MsSql
{
    public class SqlDataReader : VectorPart<SqlDataReader.Input, object[]>
    {
        private readonly string _connectionString;

        public class Input
        {
            public string SqlQuery { get; set; }
        }
        
        public SqlDataReader(string connectionString, IDefaultRegistry defaultRegistry) : base(defaultRegistry)
        {
            _connectionString = connectionString;
        }
 
        public override async Task ProcessAsync(Input data, CancellationToken cancellationToken)
        {
            var builder = new SqlConnectionStringBuilder {ConnectionString = _connectionString};
            await using var connection = new SqlConnection(builder.ConnectionString);

            await connection.OpenAsync(cancellationToken);

            await using var command = new SqlCommand(data.SqlQuery, connection);

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);

            while (await reader.ReadAsync(cancellationToken))
            {
                var values = new object[reader.FieldCount];

                reader.GetValues(values);

                this.Publish(values);
            }
        }
    }
}