using System;
using System.Threading.Tasks;
using Raven.Mission.Abstract;
using Raven.Mission.Client;
using Raven.Mission.Factories;
using Raven.Mission.RabbitMq;
using Raven.Mission.Transport;
using Raven.Serializer;

namespace Raven.Mission.ClientDemo
{
    public class DemoClient
    {

        private static DemoClient _democlient;
        private static IMissionClient _missionClient;

        private DemoClient()
        {
        }

        public static DemoClient Instace => _democlient;
        public static void Init(RabbitMissionConfig config)
        {
            _democlient=new DemoClient();
            _missionClient = MissionFactory.CreateClient().UseRabbit(config);
        }

        public async Task<DemoResponse> DemoInvoke(DemoRequest request,int timeout=30)
        {
            return await _missionClient.ExcuteAsync<DemoResponse, DemoRequest>( "order/getorder", request, timeout);
        }

        public  static void Stop()
        {
            _missionClient.Dispose();
        }
    }
}
