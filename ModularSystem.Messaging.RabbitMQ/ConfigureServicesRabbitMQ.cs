using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularSystem.Messaging.RabbitMQ.Core.DTOs;
using ModularSystem.Messaging.RabbitMQ.Core.EventBus;
using ModularSystem.Messaging.RabbitMQ.EventBus;
using ModularSystem.Messaging.RabbitMQ.Extensions;
using ModularSystem.Messaging.RabbitMQ.ServiceHost;
using RabbitMQ.Client;
using System;

namespace ModularSystem.Messaging.RabbitMQ
{
    public static class ConfigureServicesRabbitMQ
    {
        public static BusBuilder AddRabbitMQ(this IServiceCollection services, 
            IConfiguration configuration,
            Action<RabbitMQOption> optionAction)
        {
            RabbitMQOption option = new RabbitMQOption();
            optionAction(option);

            var factory = new ConnectionFactory();
            var section = configuration.GetSection(option.ConnectionFactory);
            section.Bind(factory);

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var queues = new Queues(channel);
            services.AddSingleton(channel);
            services.AddScoped<IRabbitMQEventBus, RabbitMQEventBus>();
            services.AddScoped<PublishMessage>();
            services.AddSingleton(queues);
            services.AddSingleton((RabbitMQTrackException)option);

            var serverProvaider = services.BuildServiceProvider();

            IModel bus = serverProvaider.GetRequiredService<IModel>();

            return new BusBuilder(bus, services);
        }

        public static string GetQueueName<T>() => $"{typeof(T).Name}";
    }
}
