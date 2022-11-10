using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Service
{
    public class ImdbStatusService : IHostedService, IDisposable
    {
        private Timer timer;
        private readonly ILogger<ImdbStatusService> logger;


        public ImdbStatusService(ILogger<ImdbStatusService> logger)
        {
            this.logger = logger;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(o => {
                logger.LogInformation($"IMDB STATUS Api is working");
                //action
            },
              null,
              TimeSpan.Zero,
              TimeSpan.FromSeconds(60));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
