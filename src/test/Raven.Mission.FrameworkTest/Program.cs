using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Raven.Mission.Client;
using Raven.Mission.ClientDemo;
using Raven.Mission.RabbitMq;

namespace Raven.Mission.FrameworkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DemoClient.Init("http://localhost:9008/",new RabbitMqConfig(uri:"amqp://127.0.0.1",autoDelete:true));
            var list=new List<Task>();

            //GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;
            while (true)
            {
                Console.WriteLine("press any key to start test,exit to exit...");
                var command = Console.ReadLine();
                if (command == "exit")
                    break;
                var watch = new Stopwatch();
                watch.Start();
                for (var i = 0; i < 10000; i++)
                {
                    var request = new DemoRequest
                    {
                        OrderNo = i.ToString(),
                        //MissionId = i.ToString()
                    };
                    //var result = DemoClient.Instace.DemoInvoke(request).Result;
                    //Console.WriteLine(result.Id+","+result.Name);
                    list.Add(DemoClient.Instace.DemoInvoke(request));
                }

                Task.WaitAll(list.ToArray());
                watch.Stop();
                Console.WriteLine(watch.ElapsedMilliseconds + "ms");
            }
            //Console.ReadLine();
            DemoClient.Stop();
        }
    }
}
