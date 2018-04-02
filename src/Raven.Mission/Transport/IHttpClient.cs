using System;
using System.Threading.Tasks;

namespace Raven.Mission.Transport
{
    public interface IHttpClient:IDisposable
    {
        Task SendAsync<TRequest>(string resource, TRequest request);
    }
}
