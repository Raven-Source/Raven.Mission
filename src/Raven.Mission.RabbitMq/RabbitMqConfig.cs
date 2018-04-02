using Raven.Mission.Abstract;
using Raven.Serializer;

namespace Raven.Mission.RabbitMq
{
    /// <summary>
    /// RabbitMq相关配置项
    /// </summary>
    public class RabbitMissionConfig:IMissionConfig
    {
        /// <summary>
        /// 构造RabbitMq配置
        /// </summary>
        /// <param name="rabbitHost">RabbitMq服务连接uri</param>
        /// <param name="serverHost">服务端域名</param>
        /// <param name="needAck">是否需要ack，默认true</param>
        /// <param name="workerCount">最大工作者数量</param>
        /// <param name="autoDelete">无客户端连接时，是否自动删除队列</param>
        /// <param name="serializerType"></param>
        public RabbitMissionConfig(string rabbitHost,string serverHost="", bool needAck=true,ushort workerCount=100,bool autoDelete=true,SerializerType serializerType=SerializerType.NewtonsoftJson)
        {
            Uri = rabbitHost;
            NeedAck = needAck;
            WorkerCount = workerCount;
            AutoDelete = autoDelete;
            SerializerType = serializerType;
            ServerHost = serverHost;
        }
        /// <summary>
        /// Uri   amqp://user:pass@host:port/  服务端客户端需保持一致
        /// </summary>
        public string Uri { get; set; }
        /// <summary>
        /// 是否需要ack
        /// </summary>
        public bool NeedAck { get; set; }
        /// <summary>
        /// 最大工作者数量（如需要ack时，同时unacked的最大数量）
        /// </summary>
        public ushort WorkerCount { get; set; }
        /// <summary>
        /// 无消费端时自动删除队列
        /// </summary>
        public bool AutoDelete { get; set; }

        public string ServerHost { get; set; }
        public SerializerType SerializerType { get; set; }
    }
}
