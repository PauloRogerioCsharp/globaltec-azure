using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Personal.azureFramework
{
    /// <summary>
    /// Gerencia uma database Cosmos no Azure Cloud.
    /// </summary>
    public class CosmosbdService
    {
        private static CosmosbdService _instance;
        private Dictionary<String, Container> _containers = new();
        CosmosClient _client;
        Database _database;
        private static bool _inicializado= false;


        private CosmosbdService() { }

        public static CosmosbdService Get() {
            if (_instance == null)
            {
                _instance = new CosmosbdService();
            }
            return _instance;
        }

        /// <summary>
        /// Inicializa o cliente cosmos se ele já não foi inicializado.
        /// </summary>
        public void Initialize(string endPoint, string primaryKey)
        {
            if (!CosmosbdService._inicializado)
            {
                _client = new CosmosClient(endPoint, primaryKey); 
            }


        }

        /// <summary>
        /// Cria uma data base cosmos com o nome especificado
        /// </summary>
        /// <param name="name">nome da base de dados</param>
        /// <param name="rus"> limite de solicitações </param>
        /// <returns></returns>

        public async Task CreateDatabase(string name, int rus)
        {

            _database = await this._client.CreateDatabaseIfNotExistsAsync(name, rus);
        }


        /// <summary>
        /// Cria um container no banco de dados cosmos com o nome especificado e o caminho da chave de partição
        /// </summary>
        /// <param name="name">nome do container a ser criado</param>
        /// <param name="keyPath">caminho da chave de partição</param>
        /// <returns></returns>
        public async Task CreateContainer(string name, string keyPath)
        {
  
            Container c = await _database.CreateContainerIfNotExistsAsync(name, keyPath);
            _containers.Add( name, c);

        }

        /// <summary>
        /// Adiciona um item no container cosmos com base no nome do container
        /// </summary>
        /// <typeparam name="T">Tipo de objeto</typeparam>
        /// <param name="obj">valor do objeto</param>
        /// <param name="key">valor da chave de partição</param>
        /// <returns></returns>
        public async Task<T> AddItem<T>( string containerName, T obj, object key)
        {
           return await _containers[containerName].CreateItemAsync<T>(obj, new PartitionKey(key.ToString()));

        }
    }
}
