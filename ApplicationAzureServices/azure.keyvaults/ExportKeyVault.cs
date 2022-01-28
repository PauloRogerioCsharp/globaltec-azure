using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personal.azureFramework;

namespace Personal.azure
{
    public class ExportKeyVault : KeyVaultConfigurator<ExportKeyVault>
    {

        /// <summary>
        /// Retorna o nome do keyvault criado no azure.
        /// </summary>
        /// <returns></returns>
        protected override string GetKeyVaultName()
        {
            string keyVaultName = Environment.GetEnvironmentVariable("EXPORT_KEY_VAULT_NAME").ToString();
            return keyVaultName;
        }
        /// <summary>
        /// Retorna o nome das chaves que estão armazenadas no azure.
        /// </summary>
        /// <returns></returns>
        protected override List<string> GetPropertyList()
        {
            List<string> properties = new();
            properties.Add("endpoint-cosmos-exportador");
            properties.Add("primarykey-cosmos-exportador");
            properties.Add("conectionstring-queue-export-depositos");
            properties.Add("conectionstring-queue-export-restricoes");

            return properties;
        }

        /// <summary>
        /// Retorna o usuário da api atual
        /// </summary>
        /// <returns></returns>
        protected override string GetUsuario()
        {
            return "";
        }
    }
}
