using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using System.Data.SqlClient;
using System.Threading;
using Personal.export.domain;
using Personal.azureFramework;
using Personal.azure;

namespace integrationgbl
{
   
   
class Program
    {



        async static Task Main(string[] args)
        {
            try
            {
                CosmosbdService.Get().Initialize(  ExportKeyVault.Get().GetConfig("endpoint-cosmos-exportador"), ExportKeyVault.Get().GetConfig("primarykey-cosmos-exportador"));
                AppService a = new(CosmosbdService.Get());
                await a.InitializeAsync();
                while (true)
                {
                    await a.ReadDepositosAsync();
                    await a.ReadRestricoesAsync();
                    await Task.Delay(100);
                    Console.WriteLine($"Dados lidos em {DateTime.Now}");
                }


	
            }
            catch (Exception e)
            {
				Console.WriteLine(e.StackTrace);
				throw;
            }

			Console.WriteLine("Execução finalizada.");
			Console.ReadLine();
          
        }



    }
}
