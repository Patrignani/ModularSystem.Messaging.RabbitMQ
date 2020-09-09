using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using ModularSystem.Messaging.RabbitMQ;
using ModularSystem.Messaging.RabbitMQ.Core.Command;
using Test.Signature.Command.User;
using System.Collections.Generic;
using Test.Signature.DTOs;
using Test.Console.Business.Handle;
using Microsoft.AspNetCore.Builder;
using ModularSystem.Messaging.RabbitMQ.ServiceHost;

namespace Test.Console.Business
{
    public static class Host
    {
        private const string _appsettings = "appsettings.json";

        public static IHost CreateHost()
        {
            var host = new HostBuilder()
             .UseContentRoot(Directory.GetCurrentDirectory())
             //.ConfigureHostConfiguration(configHost =>
             //{
             //    configHost.SetBasePath(Directory.GetCurrentDirectory());
             //})
             .ConfigureAppConfiguration((hostContext, configApp) =>
             {
                 hostContext.HostingEnvironment.EnvironmentName = "Test.API";
                 configApp.SetBasePath(Directory.GetCurrentDirectory());
                 configApp.AddJsonFile(_appsettings, optional: true);
                 configApp.AddJsonFile(
                     $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                     optional: true);
             })
             .ConfigureServices((hostContext, services) =>
             {
                 services.AddLogging();
                 services.AddHostedService<Services2>();
                 services.AddRabbitMQ(hostContext.Configuration, options => {
                     options.AddConnection("rabbitmq");
                     options.AddTrackException((e) =>
                     {
                         var sp = services.BuildServiceProvider();
                         var logger = sp.GetService<ILogger<UserHandle>>();
                         logger.LogError(e.GetBaseException().Message);
                     });
                 })
                  .RequestReplyCommand<UserCommandGetAll, ICollection<User>, UserHandle>()
                  .RequestReplyCommand<UserCommandGet, User, UserHandle>()
                  .SubscribeToCommand<UserCommandDelete, UserHandle>()
                  .RequestReplyCommand<UserCommandInsert, UserId, UserHandle>()
                  .SubscribeToCommand<UserCommandUpdate, UserHandle>();

                 services.AddScoped<ICommandHandler<UserCommandGetAll, ICollection<User>>, UserHandle>();
                 services.AddScoped<ICommandHandler<UserCommandGet, User>, UserHandle>();
                 services.AddScoped<ICommandHandler<UserCommandDelete>, UserHandle>();
                 services.AddScoped<ICommandHandler<UserCommandInsert, UserId>, UserHandle>();
                 services.AddScoped<ICommandHandler<UserCommandUpdate>, UserHandle>();

             })
               .ConfigureLogging((hostContext, configLogging) =>
               {
                   configLogging.AddConsole();

               })
                 .UseSerilog((context, configuration) =>
                 {
                     configuration
                         .Enrich.FromLogContext()
                         .WriteTo.File(@"Redis.myLIMSweb.Subscriber_log.txt")
                         .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate);
                 })
              .UseConsoleLifetime()
             .Build();

            //host.Services
            // .UseRabbitMq()
            // .RequestReplyCommand<UserCommandGetAll, ICollection<User>>()
            // .RequestReplyCommand<UserCommandGet, User>()
            // .SubscribeToCommand<UserCommandDelete>()
            // .RequestReplyCommand<UserCommandInsert, UserId>()
            // .SubscribeToCommand<UserCommandUpdate>();

            return host;
        }
    }
}
