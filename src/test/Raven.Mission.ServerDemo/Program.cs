using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Raven.Mission.Factories;
using Raven.Mission.RabbitMq;
using Raven.Mission.Server;
using Raven.Serializer;
using Raven.Serializer.WithMessagePack;
using Raven.Serializer.WithNewtonsoft;

namespace Raven.Mission.ServerDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            var serializer = SerializerFactory.Create(SerializerType.NewtonsoftJson);
            var queue = MiddleWareFactory.Instance.CreateRabbit(new RabbitMqConfig("amqp://127.0.0.1"), serializer);
            var server = MissionServerFactory.Create(queue,serializer);
            Container.Server = server;
            //GCSettings.LatencyMode = GCLatencyMode.LowLatency;
            using (WebApp.Start<Startup>(url: "http://localhost:9008/"))
            {
                Console.WriteLine("host:{0}", "http://localhost:9008/");
                Console.WriteLine("Press [enter] to quit...");
                Console.ReadLine();
                server.Dispose();
            }
        }
    }

    public class Container
    {
        /// <summary>
        /// DI 懒得详写
        /// </summary>
        public static MissionServer Server { get; set; }
    }
}
