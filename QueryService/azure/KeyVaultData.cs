using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personal.AzureFramework;

namespace UAU.Fiscal.QueryService.azure
{
    public class KeyVaultData : KeyVaultConfigurator<KeyVaultData>
    {

        /// <summary>
        /// Retorna o nome do keyvault criado no azure.
        /// </summary>
        /// <returns></returns>
        protected override string GetKeyVaultName()
        {
            string keyVaultName = Environment.GetEnvironmentVariable("KEY_VAULT_NAME").ToString();
            if (String.IsNullOrEmpty(keyVaultName))
            {
                return "contabil-keys";
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
            properties.Add("conectionstring-cosmos-exportador-contabil");
            properties.Add("endpoint-cosmos-exportador-contabil");
            properties.Add("primarykey-cosmos-exportador-contabil");

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
