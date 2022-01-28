using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UAU.Fiscal.QueryService.domain;

namespace UAU.Fiscal.QueryService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SqlQueryService s = new SqlQueryService();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await s.ReadDepositosAsync(_logger);
                    await s.ReadRestricoesAsyn(_logger);

                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(1000, stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message, null);
                    throw;
                }

            }
        }
    }
}
