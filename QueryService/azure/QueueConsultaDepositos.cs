using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureFramework;
using UAU.Fiscal.Document.Export.objects;


namespace UAU.Fiscal.QueryService.domain
{
    public class QueueConsultaDepositos : MessageQueue<QueueConsultaDepositos, List<ProcessamentoDeposito>>
    {
        public override string GetConnectionString()
        {
            return KeyVaultServiceQueues.Get().Configs["conectionstring-queue-query-depositos"].ToString();
        }

        public override string GetName()
        {
            string name = Environment.GetEnvironmentVariable("QUEUE_CONSULTA_DEPOSITOS").ToString();
            return name;
        }

        public override string GetResponseConnectionString()
        {
            return KeyVaultServiceQueues.Get().Configs["conectionstring-queue-export-depositos"].ToString();
        }

        public override string GetResponseName()
        {
            string name = Environment.GetEnvironmentVariable("QUEUE_CONSULTA_DEPOSITOS_RESPONSE").ToString();
            return name;
        }
    }
}
