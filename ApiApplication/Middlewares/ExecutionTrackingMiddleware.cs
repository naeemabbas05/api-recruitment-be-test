using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Middlewares
{
    public class ExecutionTrackingMiddleware

    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExecutionTrackingMiddleware> logger;
        public ExecutionTrackingMiddleware(RequestDelegate next, ILogger<ExecutionTrackingMiddleware> logger)
        {
            _next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await _next(context);
            stopwatch.Stop();

            logger.LogInformation($"Request take  {stopwatch.Elapsed} time to complete");
        }
    }
}
