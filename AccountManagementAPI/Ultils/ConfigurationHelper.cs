using Microsoft.Extensions.Configuration;

namespace AccountManagementAPI.Utils
{
    public class ConfigurationHelper
    {
        private readonly IConfiguration _config;

        public ConfigurationHelper(IConfiguration config)
        {
            _config = config;
        }

        public string GetEncryptedConnectionString()
        {
            return _config["EncryptedConnectionString"];
        }

        public string GetDecryptedConnectionString()
        {
            string encrypted = GetEncryptedConnectionString();
            return EncryptHelper.Decrypt(encrypted);
        }

        public string GetConnectionString()
        {
            return _config.GetConnectionString("Default");
        }
    }
}
