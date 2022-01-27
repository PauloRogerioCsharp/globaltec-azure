using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UAU.Fiscal.Document.Export.objects
{
    public class RestricaoBancoConta
    {
        public string id { get; set; }
        public long Empresa_BcoCont { get; set; }
        public long Banco_BcoCont { get; set; }
        public string Conta_BcoCont { get; set; }
    }
}
