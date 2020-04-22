using ModularSystem.Messaging.RabbitMQ.Core.Command;
using Test.Signature.DTOs;

namespace Test.Signature.Command.User
{
    public class UserCommandInsert : Command<UserId>
    {
        public DTOs.User User {get;set;}
    }
}
