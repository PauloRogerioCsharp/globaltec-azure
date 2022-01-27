using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAU.Fiscal.Document.Export.objects;

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
        public async Task ReadDepositosAsync() {

            var q = QueueConsultaDepositos.Get();
            var m = await q.GetMessageAsync();
            long empresa = long.Parse(m);
            SqlQuery query = new SqlQuery();
            if (empresa !=0)
            {
                List<ProcessamentoDeposito> l = await query.FindDepositosByEmpresaAsync(empresa);

                if (l.Count > 0)
                {
                    await q.InsertResponseMessageAsync(l);
                }

            }
            
        
        }

        /// <summary>
        /// Le as restricoes a serem consultadas na fila do azure
        /// </summary>
        public async Task ReadRestricoesAsyn() {

            if (_executed)
            {
                return;
            }
            var q = QueueConsultaRestricoes.Get();
            var m = await q.GetMessageAsync();

            SqlQuery query = new SqlQuery();
            

            if (m != null)
            {
                List<RestricaoBancoConta> l = await query.FindRestricoesAsync();
                _executed = true;

                if (l.Count > 0)
                {
                    await q.InsertResponseMessageAsync(l);
                    
                }
            }



        }
    }
}
