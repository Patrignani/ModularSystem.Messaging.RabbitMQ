
namespace Test.Signature.Command.User
{
    public class UserCommandDelete : ModularSystem.Messaging.RabbitMQ.Core.Command.Command
    {
        public int Id { get; set; }
    }
}
