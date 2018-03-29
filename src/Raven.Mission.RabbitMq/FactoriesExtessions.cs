using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Mission.Abstract;
using Raven.Mission.Factories;
using Raven.Mission.Server;
using Raven.Mission.Transport;
using Raven.Serializer;

namespace Raven.Mission.RabbitMq
{
    public static class MissionExtessions
    {

        /// <summary>
        /// 使用rabbitmq,针对同一服务，只允许调用一次
        /// </summary>
        /// <param name="server"></param>
        /// <param name="config">rabbitmq配置</param>
        /// <returns></returns>
        public static IMissionServer UseRabbit(this IMissionServer server, RabbitMissionConfig config)
        {
            var middleWare = new RabbitMqMiddleWare(config);
            var serializer = SerializerFactory.Create(config.SerializerType);
            server.UseMiddleWare(middleWare, serializer);
            return server;
        }

        /// <summary>
        /// 使用rabbitmq,针对同一客户端，只允许调用一次
        /// </summary>
        /// <param name="client"></param>
        /// <param name="config">rabbitmq配置</param>
        /// <returns></returns>
        public static IMissionClient UseRabbit(this IMissionClient client, RabbitMissionConfig config)
        {
            var middleWare = new RabbitMqMiddleWare(config);
            var serializer = SerializerFactory.Create(config.SerializerType);
            client.UseMiddleWare(middleWare,serializer);
            client.UseHttpClient(config.ServerHost);
            client.StartSubscribe();
            return client;
        }
    }
}
