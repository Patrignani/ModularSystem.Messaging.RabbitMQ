using System;
using System.Collections.Generic;
using System.Text;

namespace ModularSystem.Messaging.RabbitMQ.Core.Command
{
    public abstract class Command<TResult> : ICommand
    {
    }

    public abstract class Command : ICommand
    {
    }

    public interface ICommand
    { 
    }
}
