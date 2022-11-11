using ApiApplication.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Service
{
    public class ImdbStatusService : IHostedService
    {
        private Timer timer;
        private readonly ILogger<ImdbStatusService> logger;
        public static IMDB_Status imdb_Status = new IMDB_Status();

        public ImdbStatusService(ILogger<ImdbStatusService> logger)
        {
            this.logger = logger;
        }
     
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(o => {
                logger.LogInformation($"IMDB STATUS Api is working");
                //action
                var ping = new System.Net.NetworkInformation.Ping();

                var result = ping.Send("www.imdb.com");

                if (result.Status == IPStatus.Success)
                {
                    imdb_Status.last_call = DateTime.Now;
                    imdb_Status.up = true;
                }
                else {
                    imdb_Status.last_call = DateTime.Now;
                    imdb_Status.up = false;
                }
                logger.LogInformation($"IMDB#Up{imdb_Status.up}&Last Call {imdb_Status.last_call}");

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
