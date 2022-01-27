using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personal.AzureFramework;

namespace UAU.Fiscal.QueryService.domain
{
    public class KeyVaultServiceQueues : KeyVaultConfigurator<KeyVaultServiceQueues>
    {

        /// <summary>
        /// Retorna o nome do keyvault criado no azure.
        /// </summary>
        /// <returns></returns>
        protected override string GetKeyVaultName()
        {
            string keyVaultName = Environment.GetEnvironmentVariable("KEY_VAULT_SERVICE_QUEUE_NAME").ToString();
            
            if (String.IsNullOrEmpty(keyVaultName))
            {
                return "queues-vault";
            }
            else {

                return keyVaultName;
            }
        }
        /// <summary>
        /// Retorna o nome das chaves que estão armazenadas no azure.
        /// </summary>
        /// <returns></returns>
        protected override List<string> GetPropertyList()
        {
            List<string> properties = new();
            properties.Add("conectionstring-queue-query-depositos");
            properties.Add("conectionstring-queue-query-restricoes");
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
            return "paulo";
        }
    }
}
