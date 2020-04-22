using System.Threading.Tasks;
using RabbitMQ.Client;
using System;
using ModularSystem.Messaging.RabbitMQ.Extensions;
using ModularSystem.Messaging.RabbitMQ.Core.EventBus;
using ModularSystem.Messaging.RabbitMQ.Core.DTOs;
using ModularSystem.Messaging.RabbitMQ.Core.Command;

namespace ModularSystem.Messaging.RabbitMQ.EventBus
{
    public class RabbitMQEventBus : IRabbitMQEventBus
    {
        private readonly IModel _channel;
        private readonly PublishMessage _publishMessage;

        public RabbitMQEventBus(IModel channel, PublishMessage publishMessage)
        {
            _channel = channel;
            _publishMessage = publishMessage;
        }

        public async Task<RabbitRPC<TResult>> PublishWaitAsync<TResult>(Command<TResult> command, string queueName = null)
        {
            var @return = new RabbitRPC<TResult>();
            try
            {
                @return.MessageTask.StartCounting();
                queueName = string.IsNullOrEmpty(queueName) ? command.GetType().Name : queueName;

                @return.MessageTask.SetQueueName(queueName);

                if (await ExistQueue(queueName))
                {
                    @return.MessageTask.ExistQueue();
                    @return.Data = await _publishMessage.SendAsync(_channel, command, queueName);

                }
                else
                {
                    @return.MessageTask.NotExistQueue();
                    @return.MessageTask.SetMessage("Queue non-existent");
                }
            }
            catch (Exception e)
            {
                @return.MessageTask.SetMessage(e.GetBaseException().Message);
            }
            finally
            {
                @return.MessageTask.StopCounting();
            }

            return @return;
        }


        public async Task<bool> ExistQueue(string queueName)
        {
            var exist = false;

            try
            {
                _channel.QueueDeclarePassive(queueName);
                exist = true;
            }
            catch
            {

            }

            return await Task.FromResult(exist);
        }

        public async Task PublishForgetAsync(Command command)
        {
            _publishMessage.Publish(_channel, command);
            await Task.CompletedTask;
        }

        public void PublishForget(Command command)
        {
            _publishMessage.Publish(_channel, command);
        }
    }
}
