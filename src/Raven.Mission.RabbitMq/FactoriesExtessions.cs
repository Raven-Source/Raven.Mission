using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Mission.Factories;
using Raven.Mission.Transport;
using Raven.Serializer;

namespace Raven.Mission.RabbitMq
{
    public static class FactoriesExtessions
    {
        /// <summary>
        /// 创建RabbitMq中间件
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="config">RabbitMq配置</param>
        /// <param name="serializer">使用的序列化器</param>
        /// <returns></returns>
        public static IMiddleWare CreateRabbit(this MiddleWareFactory factory, RabbitMqConfig config,IDataSerializer serializer)
        {
            return new RabbitMqMiddleWare(serializer,config);
        }
    }
}
