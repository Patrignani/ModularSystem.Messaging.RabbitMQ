using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ModularSystem.Messaging.RabbitMQ.Core.Command;
using ModularSystem.Messaging.RabbitMQ.Core.DTOs;
using ModularSystem.Messaging.RabbitMQ.Extensions;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace ModularSystem.Messaging.RabbitMQ.ServiceHost
{
    public static class ServiceHost
    {
        public static BusBuilder UseRabbitMq(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            IModel bus = scope.ServiceProvider.GetRequiredService<IModel>();


            return new BusBuilder(scope, bus);
        }

        public static BusBuilder UseRabbitMq(this IServiceProvider applicationServices)
        {
            var scope = applicationServices.GetService<IServiceScopeFactory>().CreateScope();
            IModel bus = scope.ServiceProvider.GetRequiredService<IModel>();


            return new BusBuilder(scope, bus);
        }

    }

    public class BusBuilder
    {
        private readonly IServiceScope _scope;
        private readonly IModel _bus;
        private readonly RabbitMQTrackException _trackException;

        public BusBuilder(IServiceScope serviceProvider, IModel bus)
        {
            _scope = serviceProvider;
            _bus = bus;
            _trackException = _scope.ServiceProvider.GetService<RabbitMQTrackException>();
        }

        public BusBuilder RequestReplyCommand<TCommand, TCommandResult>() where TCommand : Command<TCommandResult>
        {
           
            var handler = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<TCommand, TCommandResult>>();

            _bus.WithRequestReply(handler, _trackException);
            return this;
        }

        public BusBuilder SubscribeToCommand<TCommand>() where TCommand : Command
        {
            var handler = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<TCommand>>();

            _bus.SubscribeToCommand(handler);
            return this;
        }

    }

}
