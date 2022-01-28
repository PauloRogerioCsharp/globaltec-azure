using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personal.azureFramework;
using Personal.azure.keyvault;
using Personal.azure.objects;


namespace Personal.azure.queues
{
    public class QueueConsultaDepositos : MessageQueue<QueueConsultaDepositos, List<ProcessamentoDeposito>>
    {
        public override string GetConnectionString()
        {
            return BackGroundSqlQueryKeyVault.Get().GetConfig("conectionstring-queue-query-depositos");
        }

        public override string GetName()
        {
            string name = Environment.GetEnvironmentVariable("QUEUE_CONSULTA_DEPOSITOS").ToString();
            return name;
        }

        public override string GetResponseConnectionString()
        {
            return ExportKeyVault.Get().GetConfig("conectionstring-queue-export-depositos").ToString();
        }

        public override string GetResponseName()
        {
            string name = Environment.GetEnvironmentVariable("QUEUE_EXPORT_DEPOSITOS_NAME").ToString();
            return name;
        }
    }
}
