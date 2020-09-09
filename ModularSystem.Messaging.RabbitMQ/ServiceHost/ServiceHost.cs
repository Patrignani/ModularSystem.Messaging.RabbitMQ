using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ModularSystem.Messaging.RabbitMQ.Core.Command;
using ModularSystem.Messaging.RabbitMQ.Core.DTOs;
using ModularSystem.Messaging.RabbitMQ.Core.Enum;
using ModularSystem.Messaging.RabbitMQ.Extensions;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;

namespace ModularSystem.Messaging.RabbitMQ.ServiceHost
{
    public static class ServiceHost
    {
        public static BusBuilder UseRabbitMq(this IServiceCollection services)
        {
            var serverProvaider = services.BuildServiceProvider();
            IModel bus = serverProvaider.GetRequiredService<IModel>();

            return new BusBuilder(bus, services);
        }

        //public static BusBuilder UseRabbitMq(this IApplicationBuilder app)
        //{
        //    var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
        //    IModel bus = scope.ServiceProvider.GetRequiredService<IModel>();


        //    return new BusBuilder(scope, bus);
        //}

        //public static BusBuilder UseRabbitMq(this IServiceProvider applicationServices)
        //{
        //    var scope = applicationServices.GetService<IServiceScopeFactory>().CreateScope();
        //    IModel bus = scope.ServiceProvider.GetRequiredService<IModel>();


        //    return new BusBuilder(scope, bus);
        //}

    }

    public class BusBuilder
    {
        private IServiceScope _scope
        {
            get
            {
                var serverProvaider = _serviceCollection.BuildServiceProvider();
                return serverProvaider.GetService<IServiceScopeFactory>().CreateScope();
            }
        }
        private readonly IModel _bus;
        private RabbitMQTrackException _trackException
        {
            get
            {
                return _scope.ServiceProvider.GetService<RabbitMQTrackException>(); ;
            }
        }
        private readonly IServiceCollection _serviceCollection;

        public BusBuilder(IModel bus, IServiceCollection serviceCollection)
        {
            _bus = bus;
            _serviceCollection = serviceCollection;
        }

        public BusBuilder RequestReplyCommand<TCommand, TCommandResult, THandler>(IocTypes types = IocTypes.Scoped)
            where TCommand : Command<TCommandResult>
            where THandler : ICommandHandler<TCommand, TCommandResult>
        {
            AddIocResult<TCommand, TCommandResult, THandler>(types);

            var handler = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<TCommand, TCommandResult>>();

            _bus.WithRequestReply(handler, _trackException);
            return this;
        }

        public BusBuilder SubscribeToCommand<TCommand, THandler>(IocTypes types = IocTypes.Scoped)
            where TCommand : Command
            where THandler : ICommandHandler<TCommand>
        {
            AddIocSubscribe<TCommand, THandler>(types);

            var handler = _scope.ServiceProvider
                .GetRequiredService<ICommandHandler<TCommand>>();

            _bus.SubscribeToCommand(handler);
            return this;
        }

        private void AddIocResult<TCommand, TCommandResult, THandler>(IocTypes types = IocTypes.Scoped)
            where TCommand : Command<TCommandResult>
            where THandler : ICommandHandler<TCommand, TCommandResult>
        {
            var command = typeof(ICommandHandler<TCommand, TCommandResult>);
            var handler = typeof(THandler);
            AddIoc(command, handler);
        }

        private void AddIocSubscribe<TCommand, THandler>(IocTypes types = IocTypes.Scoped)
         where TCommand : Command
         where THandler : ICommandHandler<TCommand>
        {
            var command = typeof(ICommandHandler<TCommand>);
            var handler = typeof(THandler);
            AddIoc(command, handler);
        }

        private void AddIoc(Type command, Type handler, IocTypes types = IocTypes.Scoped)
        {
            switch (types)
            {
                case IocTypes.Scoped:
                    _serviceCollection.AddScoped(command, handler);
                    break;
                case IocTypes.Singleton:
                    _serviceCollection.AddSingleton(command, handler);
                    break;
                case IocTypes.Transien:
                    _serviceCollection.AddTransient(command, handler);
                    break;
            }
        }
    }

}
