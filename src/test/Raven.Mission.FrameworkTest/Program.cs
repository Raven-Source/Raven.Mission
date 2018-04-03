using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
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
            ServicePointManager.DefaultConnectionLimit = 100;
            //DemoClient.Init(new RabbitMissionConfig("amqp://127.0.0.1", "http://localhost:9008/", serializerType: SerializerType.MessagePack), new Logger());
            DemoClient.Init("rabbit",new Logger());
            var j = 0;
            while (j<10000)
            {
                j++;
                var list = new List<Task>();
                var watch = new Stopwatch();
                watch.Start();
                for (var i = 0; i < 10000; i++)
                {
                    var request = new DemoRequest
                    {
                        OrderNo = i.ToString(),
                    };
                    list.Add(Task.Run(async () => await DemoClient.Instace.DemoInvoke(request)));
                }

                Task.WaitAll(list.ToArray());
                watch.Stop();
                Console.WriteLine($"第{j}次，1000条请求耗时:{watch.ElapsedMilliseconds}ms");

                Task.Delay(200).Wait();

            }

            Console.WriteLine("一万次请求完成，按回车退出！");
            Console.ReadLine();
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
