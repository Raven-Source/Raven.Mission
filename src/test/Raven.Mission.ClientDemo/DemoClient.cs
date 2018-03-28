using System;
using System.Threading.Tasks;
using Raven.Mission.Client;
using Raven.Mission.Factories;
using Raven.Mission.RabbitMq;
using Raven.Mission.Transport;
using Raven.Serializer;

namespace Raven.Mission.ClientDemo
{
    public class DemoClient
    {
        private static IHttpClient _client;

        private static DemoClient _democlient;
        private static MissionClient _missionClient;

        private DemoClient()
        {
        }

        public static DemoClient Instace => _democlient;
        public static void Init(string host,RabbitMqConfig config)
        {
            _democlient=new DemoClient();
            _client=new HttpClientWrapper(host);
            var serialize = SerializerFactory.Create(SerializerType.NewtonsoftJson);
            var middleware = MiddleWareFactory.Instance.CreateRabbit(config,serialize); //new RabbitMqMiddleWare(serialize,queue);
            _missionClient=new MissionClient(middleware,serialize);
        }

        public async Task<DemoResponse> DemoInvoke(DemoRequest request,int timeout=30)
        {
            return await _missionClient.ExcuteAsync<DemoResponse, DemoRequest>(_client, "order/getorder", request, timeout);
        }

        public  static void Stop()
        {
            _missionClient.StopAsync().Wait();
            _client.Dispose();
        }
    }
}
