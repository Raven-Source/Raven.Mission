using System.Threading.Tasks;
using Raven.Mission.Abstract;
using Raven.Mission.Factories;
using Raven.Mission.RabbitMq;

namespace Raven.Mission.TestApi
{
    public class DemoClient
    {

        private static DemoClient _democlient;
        private static IMissionClient _missionClient;

        private DemoClient()
        {
        }

        public static DemoClient Instace => _democlient;
        public static void Init(RabbitMissionConfig config,ILogger logger)
        {
            _democlient=new DemoClient();
            _missionClient = MissionFactory.CreateClient().UseRabbit(config,logger);
        }
        public static void Init(string config, ILogger logger)
        {
            _democlient = new DemoClient();
            _missionClient = MissionFactory.CreateClient().UseRabbit(config, logger);
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
