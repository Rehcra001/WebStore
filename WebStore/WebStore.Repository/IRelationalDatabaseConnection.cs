using Microsoft.Data.SqlClient;

namespace WebStore.Repository
{
    public interface IRelationalDatabaseConnection
    {
        SqlConnection SqlConnection();
    }
}
