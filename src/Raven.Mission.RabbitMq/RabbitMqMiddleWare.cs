using System;
using System.Threading.Tasks;
using Raven.Mission.Transport;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Raven.Serializer;

namespace Raven.Mission.RabbitMq
{
    /// <summary>
    /// RabbitMq作为中间件
    /// </summary>
    internal class RabbitMqMiddleWare : IMiddleWare
    {

        private volatile IConnection _connection;
        private readonly ConnectionFactory _factory;
        private readonly IDataSerializer _serializer;
        private readonly object _lockObj = new object();
        private readonly RabbitMissionConfig _config;
        private volatile IModel _channel;

        internal RabbitMqMiddleWare(RabbitMissionConfig config)
        {
            _serializer = SerializerFactory.Create(config.SerializerType);
            _config = config;
            _factory = new ConnectionFactory { Uri = new Uri(config.Uri) };
        }


        IConnection GetConnection()
        {
            if (_connection != null)
                return _connection;
            lock (_lockObj)
            {
                if (_connection == null)
                    _connection = _factory.CreateConnection();
            }
            return _connection;
        }



        private IModel GetChannel()
        {
            if (_channel != null && _channel.IsOpen)
            {
                return _channel;
            }
            lock (_lockObj)
            {
                if (_channel == null || _channel.IsClosed)
                {
                    _channel?.Dispose();
                    _channel = GetConnection().CreateModel();
                }
            }
            return _channel;
        }
        public Task PublishAsync<T>(string channel, T message)
        {
            try
            {
                var ch = GetChannel();
                var body = _serializer.Serialize(message);
                ch.BasicPublish("", channel, null, body);
            }
            catch (Exception e)
            {
                Console.WriteLine(DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + e.Message);
            }
            return Task.FromResult(0);

        }

        public Task SubscribeAsync<T>(string channel, Func<T, bool> handler)
        {
            var ch = GetChannel();
            ch.QueueDeclare(channel, false, false, _config.AutoDelete, null);
            var consumer = new EventingBasicConsumer(ch);
            consumer.Received += (model, ea) =>
            {
                try
                {

                    var body = ea.Body;
                    var message = _serializer.Deserialize<T>(body);
                    var consume = (EventingBasicConsumer)model;
                    if (handler(message) && _config.NeedAck)
                    {
                        consume.Model.BasicAck(ea.DeliveryTag, false);
                    }
                    else
                    {
                        //退回消息，并让他重新分发
                        consume.Model.BasicReject(ea.DeliveryTag, true);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + e.Message);
                }
            };
            if (_config.NeedAck)
                ch.BasicQos(0, _config.WorkerCount, false);
            ch.BasicConsume(channel, !_config.NeedAck, consumer);
            return Task.FromResult(0);
        }

        public Task UnsubscribeAsync(string channel)
        {
            return StopAsync();
        }

        public Task StopAsync()
        {
            _channel?.Dispose();
            _connection?.Close();
            return Task.FromResult(0);
        }
    }
}
