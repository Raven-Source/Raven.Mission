using System.Threading.Tasks;
using Raven.Mission.Messages;

namespace Raven.Mission.Abstract
{
    public interface IMissionServer:IEndpoint
    {
        void AsyncExecuteMission<TMessage>(IMissionMessage request, Task<TMessage> mission);
    }
}
