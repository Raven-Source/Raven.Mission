using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Mission.Messages;

namespace Raven.Mission.Abstract
{
    public interface IMissionServer:IEndpoint
    {
        void AsyncExecuteMission<TMessage>(MissionMessage request, Task<TMessage> mission);
    }
}
