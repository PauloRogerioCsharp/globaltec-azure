using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Personal.azureFramework
{
    /// <summary>
    /// Gerencia as configurações runtime comuns a toda a aplicação
    /// </summary>
    public abstract class KeyVaultConfigurator<T> where T : KeyVaultConfigurator<T>
    {

        private static KeyVaultConfigurator<T> _instance;
        private static string _usuario;
        private Dictionary<String, string> Configs { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Retorna o usuário de acesso no vault
        /// </summary>
        /// <returns></returns>
        protected abstract string GetUsuario();

        /// <summary>
        /// Retorna o nome do vault
        /// </summary>
        /// <returns></returns>
        protected abstract string GetKeyVaultName();
        /// <summary>
        /// Retorna as propriedades a serem buscadas no vault
        /// </summary>
        /// <returns></returns>
        protected abstract List<String> GetPropertyList();

        protected KeyVaultConfigurator() { }

        /// <summary>
        /// Retorna uma instância única do configurador
        /// </summary>
        /// <returns></returns>
        public static  KeyVaultConfigurator<T> Get() {

            if (_instance == null)
            {
                _instance =  (T)Activator.CreateInstance(typeof(T), new object[] {  });
                if (String.IsNullOrEmpty(_instance.GetUsuario()))
                {
                    _instance.ConfigureKeyVault(_instance.GetKeyVaultName(), _instance.GetPropertyList());
                }
                else {
                    _instance.ConfigureKeyVaultByUser(_instance.GetKeyVaultName(), _instance.GetPropertyList(), _instance.GetUsuario());
                }
            }

            return _instance;
        
        }


        /// <summary>
        /// Retorna uma instância única do configurador para um usuário específico
        /// </summary>
        /// <returns></returns>
        public static KeyVaultConfigurator<T> Get(string usuario)
        {
  
            if (_instance == null || usuario != _usuario )
            {
                
                _instance = (T)Activator.CreateInstance(typeof(T), new object[] { });
                _instance.VerifyArgument((x) => String.IsNullOrEmpty(usuario), usuario, "usuário não foi preenchido.");
                _instance.ConfigureKeyVaultByUser(_instance.GetKeyVaultName(), _instance.GetPropertyList(),usuario);
                _usuario = usuario;
            }

            return _instance;

        }

        /// <summary>
        /// Repreenche o vault da aplicação considerando na chave o novo usuário
        /// </summary>
        /// <param name="usuario"></param>
        public void ChangeUser(string usuario) {

            _instance.ConfigureKeyVaultByUser(_instance.GetKeyVaultName(), _instance.GetPropertyList(), usuario);

        }
      
        /// <summary>
        /// Retorna uma configuração pelo seu nome fazendo o cast para o tipo especificado
        /// </summary>
        /// <typeparam name="T">Tipo em qual a configuração será retornada</typeparam>
        /// <param name="name"> Nome da configuração</param>
        /// <returns></returns>
        public string GetConfig( string name) {
            
            string chave = $"{name}";

            if (!String.IsNullOrEmpty(_usuario))
            {
                chave = $"{name}-{_usuario}";
            }
            if (!Configs.ContainsKey(chave))
            {
                //se não contem tenta buscar no vault
                ConfigureKeyVaultByUser(GetKeyVaultName(), new List<string> { chave }, "");
                
                if (!Configs.ContainsKey(chave))
                {
                    return "";
                }
                
            }
            
            return Configs[chave];
        }



        /// <summary>
        /// Lança uma exceção caso o predicate retorne false
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="obj"></param>
        /// <param name="message"></param>
        private  void VerifyArgument(Predicate<object> condition, object obj, string message) {
        
        
            if(condition.Invoke(obj)) throw new ArgumentException(message);
        }
        /// <summary>
        /// Configura a lista de segredos da aplicação com base nas chaves. Presentes no keyvault
        /// quebradas por usuario exemplo: conectionstring-cosmos-exportador-contabil-joao
        /// </summary>
        /// <param name="name"></param>
        private void ConfigureKeyVaultByUser(string keyvaulName, List<string> keys, string usuario)
        {


            VerifyArgument((x) => String.IsNullOrEmpty(keyvaulName), keyvaulName, "nome do cofre de chaves não informado.");
            VerifyArgument((x) => (keys == null || keys.Count == 0), keys, "lista de chaves inválida.");

            //se contem alguma chave enão não insere novamente
            keys.ForEach(x =>
            {
                if (Configs.ContainsKey(x))
                {
                    return;
                }
            }
            );


            var identity = Environment.GetEnvironmentVariable("VAULT_ID");
            Environment.SetEnvironmentVariable("AZURE_TENANT_ID", "c48facd4-3682-429c-92de-3452520e37c8");

            var c = new DefaultAzureCredential(new DefaultAzureCredentialOptions {  ManagedIdentityClientId = identity });

            var kvUri = "https://" + keyvaulName + ".vault.azure.net";
            var client = new SecretClient(new Uri(kvUri),c);

            //Le a resposta e adiciona no dicionario
            keys.ForEach(x =>
            {
                try
                {
                    string chave = $"{x}";

                    if (!String.IsNullOrEmpty(usuario))
                    {
                        chave = $"{x}-{usuario}";
                    }

                    var r = client.GetSecret(chave);

                    if (r.Value != null)
                    {

                        
                        if (!Configs.ContainsKey(chave))
                        {
                            Configs.Add(chave, r.Value.Value);
                        }
                        
                    }
                }
                catch (Azure.RequestFailedException)
                {
                 }
                
            }

            );

        }

        /// <summary>
        /// Configura a lista de segredos da aplicação com base nas chaves.
        /// </summary>
        /// <param name="keys">chaves já previamente inseridas no keyvault do azure</param>
        private void ConfigureKeyVault(string keyvaulName, List<string> keys)
        {
            VerifyArgument((x) => String.IsNullOrEmpty(keyvaulName), keyvaulName, "nome do cofre de chaves não informado.");
            VerifyArgument((x) => (keys == null || keys.Count == 0), keys, "lista de chaves inválida.");
            ConfigureKeyVaultByUser(keyvaulName, keys, "");

        }
    }
}
