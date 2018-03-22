using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Mission.Transport
{
    public interface IMiddleWare
    {
        Task PublishAsync<T>(string channel, T message);

        Task SubscribeAsync<T>(string channel, Action<T> handler);

        Task UnsubscribeAsync(string channel);
    }
}
