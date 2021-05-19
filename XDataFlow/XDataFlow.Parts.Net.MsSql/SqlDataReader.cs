using Microsoft.Data.SqlClient;

namespace XDataFlow.Parts.Net.MsSql
{
    public class SqlDataReader : FlowPart<SqlDataReader.Input, object[]>
    {
        private readonly string _connectionString;

        public class Input
        {
            public string SqlQuery { get; set; }
        }
        
        public SqlDataReader(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void ProcessMessage(Input data)
        {
            var builder = new SqlConnectionStringBuilder {ConnectionString = _connectionString};
            using var connection = new SqlConnection(builder.ConnectionString);

            connection.Open();

            using var command = new SqlCommand(data.SqlQuery, connection);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var values = new object[reader.FieldCount];

                reader.GetValues(values);

                this.Publish(values);
            }
        }
    }
}