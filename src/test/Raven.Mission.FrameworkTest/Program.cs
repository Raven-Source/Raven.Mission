using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Raven.Mission.Abstract;
using Raven.Mission.RabbitMq;
using Raven.Mission.TestApi;
using Raven.Serializer;

namespace Raven.Mission.FrameworkTest
{
    class Program
    {

        static void Main(string[] args)
        {
            //DemoClient.Init(new RabbitMissionConfig("amqp://127.0.0.1", "http://localhost:9008/", serializerType: SerializerType.MessagePack), new Logger());
            DemoClient.Init("rabbit",new Logger());
            while (true)
            {
                var list = new List<Task>();
                var watch = new Stopwatch();
                watch.Start();
                for (var i = 0; i < 1000; i++)
                {
                    var request = new DemoRequest
                    {
                        OrderNo = i.ToString(),
                    };
                    list.Add(Task.Run(async () => await DemoClient.Instace.DemoInvoke(request)));
                }

                Task.WaitAll(list.ToArray());
                watch.Stop();
                Console.WriteLine(watch.ElapsedMilliseconds + "ms");

                Task.Delay(200).Wait();

            }
            DemoClient.Stop();
        }
        static Task DoSomething()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine(id);
            return Task.FromResult(0);
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
