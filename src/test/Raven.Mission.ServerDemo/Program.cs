using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Raven.Mission.Abstract;
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
            var config = new RabbitMissionConfig("amqp://127.0.0.1");
            var server = MissionFactory.CreateServer().UseRabbit(config);
            Container.Server = server;
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
        public static IMissionServer Server { get; set; }
    }
}
