

#if NET451
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Raven.Serializer;

namespace Raven.Mission.RabbitMq
{
    public class RabbitMissionConfigSection:ConfigurationSection
    {
        /// <summary>
        /// Uri   amqp://user:pass@host:port/  服务端客户端需保持一致
        /// </summary>
        [ConfigurationProperty(RabbitMissionConfig.URI,IsRequired = true)]
        public string Uri => this[RabbitMissionConfig.URI].ToString();

        /// <summary>
        /// 是否需要ack
        /// </summary>
        [ConfigurationProperty(RabbitMissionConfig.Need_Ack,DefaultValue = true)]
        public bool NeedAck => (bool)(this[RabbitMissionConfig.Need_Ack]);
        /// <summary>
        /// 最大工作者数量（如需要ack时，同时unacked的最大数量）
        /// </summary>
        [ConfigurationProperty(RabbitMissionConfig.Worker,DefaultValue = (ushort)100)]
        public ushort WorkerCount => (ushort)(this[RabbitMissionConfig.Worker]);
        /// <summary>
        /// 无消费端时自动删除队列
        /// </summary>
        [ConfigurationProperty(RabbitMissionConfig.Auto_Delete,DefaultValue = true)]
        public bool AutoDelete => (bool)(this[RabbitMissionConfig.Auto_Delete]);

        [ConfigurationProperty(RabbitMissionConfig.Server_Host)]
        public string ServerHost => this[RabbitMissionConfig.Server_Host].ToString();
    
        [ConfigurationProperty(RabbitMissionConfig.Serializer_Type,DefaultValue = Serializer.SerializerType.NewtonsoftJson)]
        public SerializerType SerializerType=>(SerializerType)(this[RabbitMissionConfig.Serializer_Type]);
        }
}
#endif

