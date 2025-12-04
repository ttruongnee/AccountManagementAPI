using Oracle.ManagedDataAccess.Client;
using System.Data;
using AccountManagementAPI.Utils;

namespace AccountManagementAPI.Database
{
    public class OracleDb : IOracleDb
    {
        private readonly string _connectionString;

        public OracleDb(ConfigurationHelper config)
        {
            _connectionString = config.GetDecryptedConnectionString();
        }

        public IDbConnection GetConnection()
        {
            return new OracleConnection(_connectionString);
        }
    }
}
