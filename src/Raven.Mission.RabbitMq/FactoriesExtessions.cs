using Raven.Mission.Abstract;
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
        /// <param name="logger"></param>
        /// <returns></returns>
        public static IMissionServer UseRabbit(this IMissionServer server, RabbitMissionConfig config,ILogger logger=null)
        {
            var middleWare = new RabbitMqMiddleWare(config);
            var serializer = SerializerFactory.Create(config.SerializerType);
            server.UseMiddleWare(middleWare, serializer,logger);
            return server;
        }

        /// <summary>
        /// 使用rabbitmq,针对同一服务，只允许调用一次
        /// </summary>
        /// <param name="server"></param>
        /// <param name="sectionName">rabbitmq配置节名称</param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static IMissionServer UseRabbit(this IMissionServer server, string sectionName, ILogger logger = null)
        {
            var config = ConfigUtils.GetSection<RabbitMissionConfig>(sectionName);
            return server.UseRabbit(config,logger);
        }

        /// <summary>
        /// 使用rabbitmq,针对同一客户端，只允许调用一次
        /// </summary>
        /// <param name="client"></param>
        /// <param name="config">rabbitmq配置</param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static IMissionClient UseRabbit(this IMissionClient client, RabbitMissionConfig config,ILogger logger=null)
        {
            var middleWare = new RabbitMqMiddleWare(config);
            var serializer = SerializerFactory.Create(config.SerializerType);
            client.UseMiddleWare(middleWare,serializer,logger);
            client.UseHttpClient(config.ServerHost);
            client.StartSubscribe();
            return client;
        }

        /// <summary>
        /// 使用rabbitmq,针对同一客户端，只允许调用一次
        /// </summary>
        /// <param name="client"></param>
        /// <param name="sectionName">rabbitmq配置节名称</param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static IMissionClient UseRabbit(this IMissionClient client, string sectionName, ILogger logger = null)
        {
            var config = ConfigUtils.GetSection<RabbitMissionConfig>(sectionName);
            return client.UseRabbit(config, logger);
        }
    }
}
