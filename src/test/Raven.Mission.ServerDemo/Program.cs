﻿using System;
using Microsoft.Owin.Hosting;
using Raven.Mission.Abstract;
using Raven.Mission.Factories;
using Raven.Mission.RabbitMq;
using Raven.Serializer;

namespace Raven.Mission.ServerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new RabbitMissionConfig("amqp://127.0.0.1",serializerType:SerializerType.MessagePack);
            var server = MissionFactory.CreateServer().UseRabbit(config,new Logger());
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
    class Logger : ILogger
    {
        public void Error(Exception ex)
        {
            Console.WriteLine(ex);
        }

        public void Error(string error)
        {
            Console.WriteLine(error);
        }
    }
}
