
using System;
#if !NET451
using Microsoft.Extensions.Configuration;
#endif
using Raven.Mission.Abstract;
using Raven.Serializer;

namespace Raven.Mission.RabbitMq
{
    /// <summary>
    /// RabbitMq相关配置项
    /// </summary>
    public class RabbitMissionConfig:IMissionConfig
    {
        public const string URI = "uri";
        public const string Server_Host = "serverHost";
        public const string Need_Ack = "needAck";
        public const string Worker = "worker";
        public const string Auto_Delete = "autoDelete";
        public const string Serializer_Type = "serializerType";
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

        public RabbitMissionConfig():this("")
        {
            
        }
#if NET451
        public RabbitMissionConfig(RabbitMissionConfigSection section)
        {
            Uri = section.Uri;
            NeedAck = section.NeedAck;
            WorkerCount = section.WorkerCount;
            AutoDelete = section.AutoDelete;
            SerializerType = section.SerializerType;
            ServerHost = section.ServerHost;
        }
            #endif
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
#if !NET451
        public IMissionConfig GetBySection(IConfigurationSection section)
        {
            Uri = section[URI];
            ServerHost = section[Server_Host];

            if (bool.TryParse(section[Need_Ack], out var ack))
                NeedAck = ack;
            if (ushort.TryParse(section[Worker], out var worker))
                WorkerCount = worker;
            if (bool.TryParse(section[Auto_Delete], out var autodelete))
                AutoDelete = autodelete;
            if (Enum.TryParse<SerializerType>(section[Serializer_Type], out var seriType))
                SerializerType = seriType;
            return this;
        }
#else

        public IMissionConfig GetBySection(object section)
        {
            if(!(section is RabbitMissionConfigSection missionConfig))
                throw new Exception("配置读取失败");
            return new RabbitMissionConfig(missionConfig);
        }
#endif

    }
}
