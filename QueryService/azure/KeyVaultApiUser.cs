using Personal.AzureFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UAU.Fiscal.QueryService.azure
{
    public class KeyVaultApiUser : KeyVaultConfigurator<KeyVaultData>
    {
        protected override string GetKeyVaultName()
        {
            throw new NotImplementedException();
        }

        protected override List<string> GetPropertyList()
        {
            throw new NotImplementedException();
        }

        protected override string GetUsuario()
        {
            throw new NotImplementedException();
        }
    }
}
