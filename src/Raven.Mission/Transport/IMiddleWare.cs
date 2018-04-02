using System;
using System.Threading.Tasks;

namespace Raven.Mission.Transport
{
    public interface IMiddleWare
    {
        Task PublishAsync<T>(string channel, T message);

        Task SubscribeAsync<T>(string channel, Func<T,bool> handler);

        Task UnsubscribeAsync(string channel);
        Task StopAsync();
    }
}
