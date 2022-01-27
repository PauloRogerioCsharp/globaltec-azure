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

namespace integrationgbl
{
   
   
class Program
    {



        async static Task Main(string[] args)
        {
            try
            {

                AppService a = new();
                await a.InitializeAsync();
                while (true)
                {
                    await a.ReadDepositosAsync();
                    await a.ReadRestricoesAsync();
                    await Task.Delay(10000);
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
