using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Mission.Server;
using Raven.Mission.Transport;
using Raven.Serializer;

namespace Raven.Mission.Factories
{
    public static class MissionServerFactory
    {
        public static MissionServer Create(IMiddleWare middleWare,IDataSerializer serializer)
        {
            return new MissionServer(middleWare,serializer);
        }
    }
}
