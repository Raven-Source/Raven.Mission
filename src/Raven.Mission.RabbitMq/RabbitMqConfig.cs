using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Mission.RabbitMq
{
    /// <summary>
    /// RabbitMq相关配置项
    /// </summary>
    public class RabbitMqConfig
    {
        /// <summary>
        /// 构造RabbitMq配置
        /// </summary>
        /// <param name="uri">RabbitMq服务连接uri</param>
        /// <param name="queueName">队列名称，客户端时必须填写</param>
        /// <param name="needAck">是否需要ack，默认true</param>
        /// <param name="workerCount">最大工作者数量</param>
        /// <param name="autoDelete">无客户端连接时，是否自动删除队列</param>
        public RabbitMqConfig(string uri,string queueName="",bool needAck=true,ushort workerCount=100,bool autoDelete=true)
        {
            Uri = uri;
            QueueName = queueName;
            NeedAck = needAck;
            WorkerCount = workerCount;
            AutoDelete = autoDelete;
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
        /// 队列名称，由客户端决定
        /// </summary>
        public string QueueName { get; set; }
        /// <summary>
        /// 无消费端时自动删除队列
        /// </summary>
        public bool AutoDelete { get; set; }
    }
}
