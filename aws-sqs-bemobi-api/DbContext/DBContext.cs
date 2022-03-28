using MySqlConnector;
using System;

namespace aws_sqs_bemobi_api.DbContext
{
    public class DBContext : IDisposable
    {
        public MySqlConnection Connection { get; }
        public DBContext(string connectionString) => Connection = new MySqlConnection(connectionString);

        public void Open()
        {
            if (Connection.State != System.Data.ConnectionState.Open)
                Connection.Open();
        }

        public void Close()
        {
            if (Connection.State != System.Data.ConnectionState.Closed)
                Connection.Close();
        }

        public void Dispose() => Connection.Dispose();
    }
}