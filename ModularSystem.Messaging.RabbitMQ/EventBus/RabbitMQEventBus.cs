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
            var rabbitRPC = new RabbitRPC<TResult>();
            try
            {
                rabbitRPC.MessageTask.StartCounting();
                queueName = string.IsNullOrEmpty(queueName) ? command.GetType().Name : queueName;

                rabbitRPC.MessageTask.SetQueueName(queueName);

                if (await ExistQueue(queueName))
                {
                    rabbitRPC.MessageTask.ExistQueue();
                    rabbitRPC.Data = await _publishMessage.SendAsync(_channel, command, queueName);

                }
                else
                {
                    rabbitRPC.MessageTask.NotExistQueue();
                    rabbitRPC.MessageTask.SetMessage("Queue non-existent");
                }
            }
            catch (Exception e)
            {
                rabbitRPC.MessageTask.SetMessage(e.GetBaseException().Message);
            }
            finally
            {
                rabbitRPC.MessageTask.StopCounting();
            }

            return rabbitRPC;
        }

        public async Task<TResult> PublishBasicRpcAsync<TResult>(Command<TResult> command, string queueName = null)
            where TResult : class
        {
            queueName = string.IsNullOrEmpty(queueName) ? command.GetType().Name : queueName;

            TResult data = null;

            if (await ExistQueue(queueName))
                data = await _publishMessage.SendAsync(_channel, command, queueName);

            return data;
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

        public void PublishForget(Command command, string queueName = null) => _publishMessage.Publish(_channel, command, queueName);
    }
}
