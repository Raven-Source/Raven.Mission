using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Mission.Transport
{
    public interface IHttpClient:IDisposable
    {
        Task SendAsync<TRequest>(string resource, TRequest request);
    }
}
