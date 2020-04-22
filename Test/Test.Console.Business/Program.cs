using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Topshelf;

namespace Test.Console.Business
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host=  Host.CreateHost();
            await host.RunAsync();
                //HostFactory.Run(x =>
                //{
                //    x.Service<Services>(sc =>
                //    {
                //        sc.ConstructUsing(s => host.Services.GetRequiredService<Services>());
                //        sc.WhenStarted((s, c) => s.Start(c));
                //        sc.WhenStopped((s, c) => s.Stop(c));
                //    });

                //    x.RunAsLocalSystem()
                //        .DependsOnEventLog()
                //        .StartAutomatically()
                //        .EnableServiceRecovery(rc => rc.RestartService(1));

                //    x.SetDescription("Teste de fila RabbitMq");
                //    x.SetDisplayName("Fila RabbitMq");
                //    x.SetServiceName("Test.Console.Business");
                //});

                await Task.CompletedTask;
        }
    }
}
