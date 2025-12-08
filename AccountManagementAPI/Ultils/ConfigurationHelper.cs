using Microsoft.Extensions.Configuration;

namespace AccountManagementAPI.Utils
{
    public class ConfigurationHelper
    {
        private readonly IConfiguration _config;
        private readonly EncryptHelper _encryptHelper;

        public ConfigurationHelper(IConfiguration config, EncryptHelper encryptHelper)
        {
            _config = config;
            _encryptHelper = encryptHelper;
        }

        public string GetEncryptedConnectionString()
        {
            return _config["EncryptedConnectionString"];
        }

        public string GetDecryptedConnectionString()
        {
            string encrypted = GetEncryptedConnectionString();
            return _encryptHelper.Decrypt(encrypted);
        }

        //public string GetConnectionString()
        //{
        //    return _config.GetConnectionString("Default");
        //}
    }
}
