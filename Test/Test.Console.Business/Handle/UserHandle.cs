using Microsoft.Extensions.Logging;
using ModularSystem.Messaging.RabbitMQ.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Console.Business.Service;
using Test.Signature.Command.User;
using Test.Signature.DTOs;

namespace Test.Console.Business.Handle
{
    public class UserHandle : ICommandHandler<UserCommandGetAll, ICollection<User>>,
                              ICommandHandler<UserCommandGet, User>,
                              ICommandHandler<UserCommandDelete>,
                              ICommandHandler<UserCommandInsert, UserId>,
                              ICommandHandler<UserCommandUpdate>
    {
        private MockyUser _mocky;
        private ILogger<UserHandle> _logger;
        public UserHandle(ILogger<UserHandle> logger)
        {
            _mocky = new MockyUser();
            _logger = logger;
        }

        public async Task<ICollection<User>> HandleWithEventAsync(UserCommandGetAll command)
        {
            throw new System.InvalidOperationException("aaaa");
            _logger.LogInformation($"Recebido as:{DateTime.UtcNow}");
            return await Task.FromResult(_mocky.GetAll());
        }

        public async Task<User> HandleWithEventAsync(UserCommandGet command)
        {
            throw new System.InvalidOperationException("aaaa");
            return await Task.FromResult(_mocky.GetAll()
                .Where(x => x.Id == command.Id)
                .FirstOrDefault());
        }

        public async Task HandleWithEventAsync(UserCommandDelete command)
        {

        }

        public async Task<UserId> HandleWithEventAsync(UserCommandInsert command)
        {
            return await Task.FromResult(new UserId { Id = 156 });
        }

        public async Task HandleWithEventAsync(UserCommandUpdate command)
        {

        }
    }
}
