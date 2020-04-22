using Test.Signature.DTOs;

namespace Test.Signature.Command.User
{
    public class UserCommandUpdate : ModularSystem.Messaging.RabbitMQ.Core.Command.Command
    {
        public UserUpdate User { get; set;}
    }
}
