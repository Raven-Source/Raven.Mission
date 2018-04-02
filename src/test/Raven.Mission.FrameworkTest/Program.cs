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
            DemoClient.Init(new RabbitMissionConfig("amqp://127.0.0.1", "http://localhost:9008/", serializerType: SerializerType.MessagePack),new Logger());
            var list = new List<Task>();
            while (true)
            {
                try
                {
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
                }
                catch (Exception e)
                {

                }
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
