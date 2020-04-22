using ModularSystem.Messaging.RabbitMQ.Core.Command;
using System.Collections.Generic;

namespace Test.Signature.Command.User
{
    public class UserCommandGetAll : Command<ICollection<DTOs.User>>
    {
    }
}
