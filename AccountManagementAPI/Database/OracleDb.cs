using Oracle.ManagedDataAccess.Client;
using System.Data;
using AccountManagementAPI.Utils;

namespace AccountManagementAPI.Database
{
    public class OracleDb : IOracleDb
    {
        private readonly string _connectionString;
        private static string authconnectionString = null;      

        public OracleDb(ConfigurationHelper config)
        {
            if (authconnectionString == null)
            {
                _connectionString = config.GetDecryptedConnectionString();
                authconnectionString = _connectionString;
                return;
            }
            _connectionString = authconnectionString;
        }

        public IDbConnection GetConnection()
        {
            return new OracleConnection(_connectionString);
        }
    }
}
