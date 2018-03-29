using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Mission.Abstract;
using Raven.Mission.Client;
using Raven.Mission.Server;
using Raven.Mission.Transport;
using Raven.Serializer;

namespace Raven.Mission.Factories
{
    public static class MissionFactory
    {
        /// <summary>
        /// 创建服务端
        /// </summary>
        /// <returns></returns>
        public static IMissionServer CreateServer()
        {
            return new MissionServer();
        }
        /// <summary>
        /// 创建客户端
        /// </summary>
        /// <returns></returns>
        public static IMissionClient CreateClient()
        {
            return new MissionClient();
        }
    }
}
