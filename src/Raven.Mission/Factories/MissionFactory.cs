using Raven.Mission.Abstract;
using Raven.Mission.Client;
using Raven.Mission.Server;

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
