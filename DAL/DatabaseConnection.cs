using System.Configuration;
using System.Data.SqlClient;

namespace ProjectTaskManagement.DAL
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ProjectTaskDB"].ConnectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}