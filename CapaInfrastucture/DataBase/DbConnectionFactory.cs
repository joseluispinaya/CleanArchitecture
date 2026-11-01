using Microsoft.Data.SqlClient;
using System.Data;

namespace CapaInfrastucture.DataBase
{
    public class DbConnectionFactory
    {
        private readonly string _connectionString;
        public DbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
