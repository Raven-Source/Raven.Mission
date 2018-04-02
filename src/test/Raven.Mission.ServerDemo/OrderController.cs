using System;
using System.Threading.Tasks;
using System.Web.Http;
using Raven.Mission.TestApi;

namespace Raven.Mission.ServerDemo
{
    public class OrderController:ApiController
    {
        
        [HttpPost]
        public Task<string> Test()
        {
            Task.Run(async ()=> {
                await Task.Delay(3000);
                Console.WriteLine( "done");
            });
            return Task.FromResult("test");
        }
        [HttpPost]
        public int GetOrder(DemoRequest request)
        {
            Container.Server.AsyncExecuteMission(request, Task.FromResult(new DemoResponse
            {
                Id = request.OrderNo,
                Name = "demo"
            }));
            return 0;
        }
    }
}
