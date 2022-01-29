using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Personal.azure.objects;
using Personal.azure.queues;

namespace UAU.Fiscal.QueryService.domain
{
    /// <summary>
    /// Serviço que lê a fila do azure e dispara uma consulta no banco.
    /// </summary>
    public class SqlQueryService
    {
        private static bool _executed = false;
        /// <summary>
        /// Le os depósitos a serem consultados na fila do azure
        /// </summary>
        public async Task ReadDepositosAsync(ILogger logger) {

            var q = QueueConsultaDepositos.Get();
            var m = await q.GetMessageAsync();

            SqlQuery query = new SqlQuery();
            if (m != null)
            {
                var bs = Convert.FromBase64String(m);
                m = System.Text.Encoding.UTF8.GetString(bs);
                logger.LogInformation($"Mensagem lida {m}");
         
                List<ProcessamentoDeposito> l = await query.FindDepositosByEmpresaAsync(long.Parse(m.Split(":")[0]), m.Split(":")[1]);

                if (l.Count > 0)
                {
                    await q.InsertResponseMessageAsync(l);
                }

            }
            
        
        }

        /// <summary>
        /// Le as restricoes a serem consultadas na fila do azure
        /// </summary>
        public async Task ReadRestricoesAsyn(ILogger logger) {

            var q = QueueConsultaRestricoes.Get();
            var m = await q.GetMessageAsync();
           

            SqlQuery query = new SqlQuery();
            

            if (m != null)
            {
                var bs = Convert.FromBase64String(m);
                m = System.Text.Encoding.UTF8.GetString(bs);
                logger.LogInformation($"Mensagem lida {m}");
                List<RestricaoBancoConta> l = await query.FindRestricoesAsync(m);
                _executed = true;

                if (l.Count > 0)
                {
                    await q.InsertResponseMessageAsync(l);
                    
                }
            }



        }
    }
}
