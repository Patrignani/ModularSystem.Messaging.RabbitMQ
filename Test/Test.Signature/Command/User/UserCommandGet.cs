using ModularSystem.Messaging.RabbitMQ.Core.Command;

namespace Test.Signature.Command.User
{
    public class UserCommandGet : Command<DTOs.User>
    {
        public int Id { get; set; }
    }
}
