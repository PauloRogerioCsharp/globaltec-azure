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
            return BackGroundSqlQueryKeyVault.Get().Configs["conectionstring-queue-query-restricoes"].ToString();
        }

        public override string GetName()
        {
            string name = Environment.GetEnvironmentVariable("QUEUE_CONSULTA_RESTRICOES").ToString();
            return name;
        }

        public override string GetResponseConnectionString()
        {
            return BackGroundSqlQueryKeyVault.Get().Configs["conectionstring-queue-export-restricoes"].ToString();
        }

        public override string GetResponseName()
        {
            string name = Environment.GetEnvironmentVariable("QUEUE_CONSULTA_RESTRICOES_RESPONSE").ToString();
            return name;
        }
    }
}
