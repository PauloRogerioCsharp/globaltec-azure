using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CloudFunctions
{
    public static class ExportarRestricoesBancarias
    {
        [FunctionName("ExportarRestricoesBancarias")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [Queue("queue-consulta-restricoes", Connection = "personalappexport")] ICollector<string> msg,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string usuario = req.Query["usuario"];

            log.LogInformation($"Request para usuário {usuario}");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);


            string responseMessage = "";
            if (!(string.IsNullOrEmpty(usuario)))
            {
                responseMessage = $"{usuario}";
                msg.Add(responseMessage);
                return new OkObjectResult("Solicitação processada.");
            }
            else
            {

                return new BadRequestObjectResult("Parâmetros inválidos.");
            }
        }
    }
}
