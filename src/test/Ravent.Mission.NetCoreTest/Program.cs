using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Raven.Mission.RabbitMq;
using Raven.Mission.TestApi;
using System.Net.Http;
using Raven.Mission.Abstract;

namespace Ravent.Mission.NetCoreTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DemoClient.Init(new RabbitMissionConfig("amqp://127.0.0.1", "http://localhost:9008/"),new Logger());
            var list = new List<Task>();
            while (true)
            {
                //Console.WriteLine("press any key to start test,exit to exit...");
                //var command = Console.ReadLine();
                //if (command == "exit")
                //    break;
                var watch = new Stopwatch();
                watch.Start();
                try
                {
                    for (var i = 0; i < 1000; i++)
                    {
                        var request = new DemoRequest
                        {
                            OrderNo = i.ToString(),
                        };
                        list.Add(Task.Run(async ()=>await DemoClient.Instace.DemoInvoke(request)));
                        //list.Add(Task.Run(async ()=>await DoSomething()));
                        //list.Add( DoSomething());
                        //DemoClient.Instace.DemoInvoke(request).Wait();
                        //list.Add(DemoClient.Instace.DemoInvoke(request));
                    }

                    Task.WaitAll(list.ToArray());
                }
                catch (Exception e)
                {

                }
                watch.Stop();
                Console.WriteLine(watch.ElapsedMilliseconds + "ms");
                Task.Delay(200).Wait();

            }
            DemoClient.Stop();
        }
        static HttpClient httpClient = new HttpClient() { BaseAddress = new Uri("http://192.168.2.90:9008/") };
        static async Task DoSomething()
        {
            try
            {
                var result = await httpClient.PostAsync("order/test",new StringContent("abc"));
                //Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

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
