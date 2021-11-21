using LicenceWorkorder.Models;
using LicenceWorkorder.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LicenceWorkorder
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient httpclient;
        //public Worker(ILogger<Worker> logger)
        //{
        //    _logger = logger;
        //}
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            httpclient = new HttpClient();
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            httpclient.Dispose();
            Log.Logger.Warning("Service is stopped!");
            return base.StopAsync(cancellationToken);
        }
        public override void Dispose()
        {
            base.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //while (!stoppingToken.IsCancellationRequested)
            //{
                //_logger.LogInformation("Welcome usvameria. Worker running at: {time}", DateTimeOffset.Now);
                //var client = new ClientWorker();
                //var result = client.CheckClient();
                TcpListener listener = new TcpListener();
                listener.HttpListener("12999");
                //listener.ListenTcpPort(this,stoppingToken);
                //TcpMessager.SendMessage();    
            //if (result != null)
                //{
                //   await StopAsync(stoppingToken,stoppingToken);
                //    break;
                //}
                //await Task.Delay(5000, stoppingToken);
            //}
        }
    }
}
