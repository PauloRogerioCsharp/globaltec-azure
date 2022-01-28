using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personal.azure.objects;
using Personal.azure.keyvault;
using Personal.azureFramework;

namespace Personal.azure.queues
{
    public class QueueConsultaRestricoes : MessageQueue<QueueConsultaRestricoes, List<RestricaoBancoConta>>
    {
        public override string GetConnectionString()
        {
            return BackGroundSqlQueryKeyVault.Get().GetConfig("conectionstring-queue-query-restricoes");
        }

        public override string GetName()
        {
            string name = Environment.GetEnvironmentVariable("QUEUE_CONSULTA_RESTRICOES").ToString();
            return name;
        }

        public override string GetResponseConnectionString()
        {
            return ExportKeyVault.Get().GetConfig("conectionstring-queue-export-restricoes");
        }

        public override string GetResponseName()
        {
            string name = Environment.GetEnvironmentVariable("QUEUE_EXPORT_RESTRICOES_NAME").ToString();
            return name;
        }
    }
}
