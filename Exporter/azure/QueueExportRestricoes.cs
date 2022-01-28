using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personal.azureFramework;
using Personal.azure.objects;

namespace Personal.azure.queues
{
    public class QueueExportRestricoes : MessageQueue<QueueExportRestricoes, List<RestricaoBancoConta>>
    {
        public override string GetConnectionString()
        {
            return ExportKeyVault.Get().GetConfig("conectionstring-queue-export-restricoes");
        }

        public override string GetName()
        {
            string name = Environment.GetEnvironmentVariable("QUEUE_EXPORT_RESTRICOES_NAME").ToString();
            return name;
        }

        public override string GetResponseConnectionString()
        {
            throw new NotImplementedException();
        }

        public override string GetResponseName()
        {
            throw new NotImplementedException();
        }
    }
}
