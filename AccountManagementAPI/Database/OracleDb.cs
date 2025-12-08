using Oracle.ManagedDataAccess.Client;
using System.Data;
using AccountManagementAPI.Utils;

namespace AccountManagementAPI.Database
{
    public class OracleDb : IOracleDb
    {
        private readonly string _connectionString;
        private static string authconnectionString;      

        public OracleDb(ConfigurationHelper config)
        {
            if (authconnectionString == null)
            {
                _connectionString = config.GetConnectionString();
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
