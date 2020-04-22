using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;

namespace Test.Console.Business
{
    public class Services : ServiceControl
    {
        //private Timer _timer;
        private readonly ILogger<Services> _logger;

        public Services(ILogger<Services> logger)
        {
            _logger = logger;
        }

        public bool Start(HostControl hostControl)
        {
            //  _timer = new Timer(async c => await ExecuteAsync(), null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
            _logger.LogInformation("Aqui ");
            return true;
        }

        private async Task ExecuteAsync()
        {
            try
            {
              //  _timer.Change(Timeout.Infinite, Timeout.Infinite);


                //Início da execução
            }
            finally
            {
             //   _timer.Change(TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(60));
            }
        }

        public bool Stop(HostControl hostControl)
        {
            return true;
        }
    }
}
