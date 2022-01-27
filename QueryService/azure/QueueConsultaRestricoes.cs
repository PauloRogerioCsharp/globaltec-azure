using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAU.Fiscal.Document.Export.objects;
using AzureFramework;

namespace UAU.Fiscal.QueryService.domain
{
    public class QueueConsultaRestricoes : MessageQueue<QueueConsultaRestricoes, List<RestricaoBancoConta>>
    {
        public override string GetConnectionString()
        {
            return KeyVaultServiceQueues.Get().Configs["conectionstring-queue-query-restricoes"].ToString();
        }

        public override string GetName()
        {
            string name = Environment.GetEnvironmentVariable("QUEUE_CONSULTA_RESTRICOES").ToString();
            return name;
        }

        public override string GetResponseConnectionString()
        {
            return KeyVaultServiceQueues.Get().Configs["conectionstring-queue-export-restricoes"].ToString();
        }

        public override string GetResponseName()
        {
            string name = Environment.GetEnvironmentVariable("QUEUE_CONSULTA_RESTRICOES_RESPONSE").ToString();
            return name;
        }
    }
}
