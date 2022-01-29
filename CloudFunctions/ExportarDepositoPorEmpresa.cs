using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CloudFunctions
{
    public static class ExportarDepositoPorEmpresa
    {
        [FunctionName("ExportarDepositoPorEmpresa")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Queue("queue-consulta-depositos", Connection = "personalappexport")] ICollector<string> msg,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string empresa = req.Query["empresa"];
            string usuario = req.Query["usuario"];

            log.LogInformation($"Request para empresa {empresa} e usuário {usuario}");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);


            string responseMessage = "";
            if (!(string.IsNullOrEmpty(empresa) && string.IsNullOrEmpty(usuario)))
            {
                responseMessage = $"{empresa}:{usuario}";
                msg.Add(responseMessage);
                return new OkObjectResult("Solicitação realizada.");
            } else
            {

                return new BadRequestObjectResult("Parâmetros inválidos.");
            }


           
        }
    }
}
