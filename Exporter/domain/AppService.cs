using System.Collections.Generic;
using System.Threading.Tasks;
using Personal.azure.objects;
using System.Text.Json;
using System.Linq;
using Personal.azure.queues;
using Personal.azureFramework;

namespace Personal.export.domain
{

    public class AppService
    {
        CosmosbdService _cosmos;



        public AppService() {

            _cosmos = CosmosbdService.Get();
            
        
        }

        /// <summary>
        /// Inicializa o serviço criando a base e os containers do cosmos
        /// </summary>
        /// <returns></returns>
        public async Task InitializeAsync() {
            
            await _cosmos.CreateDatabase("FatosGeradores",10000);
            await _cosmos.CreateContainer("Depositos", "/id");
            await _cosmos.CreateContainer("Restricoes", "/id");

        }

        /// <summary>
        /// Consulta os depositos e restricoes no servidor sql exportando os processamentos para 
        /// o banco de dados cosmos na nuvem
        /// </summary>
        /// <returns> task de processamento </returns>
        public async Task ExportRestricoesAsync(List<RestricaoBancoConta> restricoes)
        {

            await restricoes.ToAsyncEnumerable<RestricaoBancoConta>().ForEachAsync(async x =>
            {
                await _cosmos.AddItem<RestricaoBancoConta>("depositos", x, x.id);
            });


        }


        /// <summary>
        /// Consulta os depositos e restricoes no servidor sql exportando os processamentos para 
        /// o banco de dados cosmos na nuvem
        /// </summary>
        /// <returns> task de processamento </returns>
        public async Task ExportDepositosAsync(List<ProcessamentoDeposito> l)
        {


            await l.ToAsyncEnumerable<ProcessamentoDeposito>().ForEachAsync(async x =>
            {
                await _cosmos.AddItem<ProcessamentoDeposito>("depositos", x, x.id);

            });


        }

        /// <summary>
        /// Le os depositos postados na fila do azure
        /// </summary>
        /// <returns></returns>
        public async Task ReadDepositosAsync() {

            var q = QueueExportDepositos.Get();
            var m = await q.GetMessageAsync();
            
            if (m != null)
            {
                List<ProcessamentoDeposito> l = JsonSerializer.Deserialize<List<ProcessamentoDeposito>>(m);
                if (l.Count>0)
                {
                    await ExportDepositosAsync(l);
                }
            }

        
        
        }


        /// <summary>
        /// le as restrições postadas na fila do azure
        /// </summary>
        /// <returns></returns>
        public async Task ReadRestricoesAsync() {

            var q = QueueExportDepositos.Get();
            var m = await q.GetMessageAsync();

            if (m != null)
            {
                List<RestricaoBancoConta> l = JsonSerializer.Deserialize<List<RestricaoBancoConta>>(m);
                if (l.Count > 0)
                {
                    await ExportRestricoesAsync(l);
                }
            }



        }




    }
}
