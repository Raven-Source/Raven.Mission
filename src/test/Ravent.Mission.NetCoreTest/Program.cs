using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Raven.Mission.RabbitMq;
using Raven.Mission.TestApi;
using Raven.Mission.Abstract;
using Raven.Serializer;

namespace Ravent.Mission.NetCoreTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DemoClient.Init(new RabbitMissionConfig("amqp://127.0.0.1", "http://localhost:9008/",serializerType:SerializerType.MessagePack), new Logger());
            //TaskScheduler scheduler = TaskScheduler.Current;
            while (true)
            {
                //Console.WriteLine("press any key to start test,exit to exit...");
                //var command = Console.ReadLine();
                //if (command == "exit")
                //    break;
                var list = new List<Task>();
                var watch = new Stopwatch();
                watch.Start();
                for (var i = 0; i < 10000; i++)
                {
                    var request = new DemoRequest
                    {
                        OrderNo = i.ToString(),
                    };
                    //list.Add(Task.Factory.StartNew(async () => await DemoClient.Instace.DemoInvoke(request)));
                    list.Add(DemoClient.Instace.DemoInvoke(request));
                }

                Task.WaitAll(list.ToArray());

                watch.Stop();
                Console.WriteLine(watch.ElapsedMilliseconds + "ms");
                Task.Delay(200).Wait();

            }
            DemoClient.Stop();
        }

    }
    class Logger : ILogger
    {
        public void LogError(Exception ex, object obj)
        {
            Console.WriteLine(ex);
        }
    }
}
