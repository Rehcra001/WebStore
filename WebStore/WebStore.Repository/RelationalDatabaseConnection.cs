using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WebStore.Repository
{
    public class RelationalDatabaseConnection : IRelationalDatabaseConnection
    {
        private readonly IConfiguration _config;

        public RelationalDatabaseConnection(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection SqlConnection()
        {
            return new SqlConnection(_config.GetConnectionString("WebStoreDB"));
        }
    }
}
