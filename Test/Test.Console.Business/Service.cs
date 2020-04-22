using Microsoft.Extensions.Hosting;
using ModularSystem.Messaging.RabbitMQ;
using System.Threading;
using System.Threading.Tasks;
using Test.Signature.Command.User;

namespace Test.Console.Business
{
    public class Services2 : IHostedService
    {
        private readonly Queues _queues;

        public Services2(Queues queues)
        {
            _queues = queues;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _queues
                 .DeleteQueue<UserCommandGetAll>();



            return Task.CompletedTask;
        }
    }
}
