using ModularSystem.Messaging.RabbitMQ.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Business.Service;
using Test.Signature.Command.User;
using Test.Signature.DTOs;

namespace Test.Business.Handle
{
    public class UserHandle : ICommandHandler<UserCommandGetAll, ICollection<User>>,
                              ICommandHandler<UserCommandGet, User>,
                              ICommandHandler<UserCommandDelete>,
                              ICommandHandler<UserCommandInsert, UserId>,
                              ICommandHandler<UserCommandUpdate>
    { 
        private MockyUser _mocky;
        public UserHandle()
        {
            _mocky = new MockyUser();
        }

        public async Task<ICollection<User>> HandleWithEventAsync(UserCommandGetAll command)
        {
            return await Task.FromResult(_mocky.GetAll());
        }

        public async Task<User> HandleWithEventAsync(UserCommandGet command)
        {
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
