using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Text.Json;

namespace Personal.azureFramework
{
    public abstract class MessageQueue<I,O> where I :MessageQueue<I,O>
    {

        private QueueClient _queue;
        private QueueClient _responseQueue;
        private static MessageQueue<I,O> _instance;


        /// <summary>
        /// String de conexão do azure
        /// </summary>
        /// <returns></returns>
        public abstract string GetConnectionString();
        
        
        /// <summary>
        /// Nome da fila
        /// </summary>
        /// <returns></returns>
        public abstract string GetName();


        /// <summary>
        /// String de conexão do azure da resposta
        /// </summary>
        /// <returns></returns>
        public abstract string GetResponseConnectionString();


        /// <summary>
        /// Nome da fila de resposta
        /// </summary>
        /// <returns></returns>
        public abstract string GetResponseName();



        /// <summary>
        /// Cria instancia única da fila
        /// </summary>
        /// <returns></returns>
        public static MessageQueue<I,O> Get()
        {

            if (_instance == null)
            {
                _instance = (I) Activator.CreateInstance(typeof(I), new object[] { });
                _instance._queue = new QueueClient(_instance.GetConnectionString(), _instance.GetName());
                _instance._responseQueue = new QueueClient(_instance.GetResponseConnectionString(), _instance.GetResponseName());

            }

            return _instance;

        }

        /// <summary>
        /// Cria uma instancia de uma fila apartir da string de conexão do azure
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="name"></param>
        protected MessageQueue(){
           
        }


        /// <summary>
        /// Escreve uma mensagem na fila de resposta
        /// </summary>
        /// <param name="newMessage"></param>
        /// <returns></returns>
        public async Task InsertResponseMessageAsync(O newMessage)
        {

            if (null != await _responseQueue.CreateIfNotExistsAsync())
            {
                Console.WriteLine("A fila de resposta foi criada.");
            }

            await _responseQueue.SendMessageAsync(JsonSerializer.Serialize<O>(newMessage));
        }

        /// <summary>
        /// Escreve uma mensagem na fila
        /// </summary>
        /// <param name="newMessage"></param>
        /// <returns></returns>
        public async Task InsertMessageAsync(O newMessage)
        {

            if (null != await _queue.CreateIfNotExistsAsync())
            {
                Console.WriteLine("A fila foi criada.");
            }

            await _queue.SendMessageAsync(JsonSerializer.Serialize<O>(newMessage)) ;
        }


        /// <summary>
        /// Le a fila do azure
        /// </summary>
         /// <returns>A mensagem da fila</returns>
        public async Task<string> GetMessageAsync()
        {

            QueueProperties properties = await _queue.GetPropertiesAsync();


            //Verifica se tem mensagem na fila
            if (properties.ApproximateMessagesCount > 0)
            {
                QueueMessage[] retrievedMessage = await _queue.ReceiveMessagesAsync(1);
                
                string theMessage = retrievedMessage[0].Body.ToString();
                await _queue.DeleteMessageAsync(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
               
                return theMessage;
            }

            return null;
        }
    }
}
