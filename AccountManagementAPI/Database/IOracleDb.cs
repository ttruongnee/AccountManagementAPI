using System.Data;

namespace AccountManagementAPI.Database
{
    public interface IOracleDb
    {
        IDbConnection GetConnection();
    }
}
