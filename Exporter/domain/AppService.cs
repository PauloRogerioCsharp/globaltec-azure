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



        public AppService(CosmosbdService cosmos) {

            _cosmos = cosmos;
            
        
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
                await _cosmos.AddItem<RestricaoBancoConta>("Restricoes", x, x.id);
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
                await _cosmos.AddItem<ProcessamentoDeposito>("Depositos", x, x.id);

            });


        }

        /// <summary>
        /// Le os depositos postados na fila do azure
        /// </summary>
        /// <returns></returns>
        public async Task ReadDepositosAsync() {

            var q = QueueExportDepositos.Get();
            var lista = await q.GetAllMessagesAsync();
            
            if (lista.Count>0)
            {
                List<ProcessamentoDeposito> depositos = new(lista.Count);
                await lista.ToAsyncEnumerable().ForEachAsync(async m =>
                {
                    
                    ProcessamentoDeposito p = JsonSerializer.Deserialize<ProcessamentoDeposito>(m);
                    depositos.Add(p);

                });
                
                if (depositos.Count>0)
                {
                    await ExportDepositosAsync(depositos);
                }
            }

        
        
        }


        /// <summary>
        /// le as restrições postadas na fila do azure
        /// </summary>
        /// <returns></returns>
        public async Task ReadRestricoesAsync() {

            var q = QueueExportRestricoes.Get();
            var lista = await q.GetAllMessagesAsync();

            if (lista.Count>0)
            {

                List<RestricaoBancoConta> restricoes = new(lista.Count);
                
                lista.ForEach(m => {

                    restricoes.Add(JsonSerializer.Deserialize<RestricaoBancoConta>(m));
                
                });

                
                if (restricoes.Count > 0)
                {
                    await ExportRestricoesAsync(restricoes);
                }
            }



        }




    }
}
