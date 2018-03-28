using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Raven.Mission.ClientDemo;
using Raven.Mission.Factories;
using Raven.Mission.RabbitMq;
using Raven.Mission.Server;

namespace Raven.Mission.ServerDemo
{
    public class OrderController:ApiController
    {
        
        [HttpGet]
        public string Test()
        {
            return "test";
        }
        [HttpPost]
        public Task<int> GetOrder(DemoRequest request)
        {
            Container.Server.AsyncExecuteMission(request,Task.FromResult(new DemoResponse
            {
                Id = request.OrderNo,
                Name = "demo"
            }));
            return Task.FromResult(0);
        }
    }
}
