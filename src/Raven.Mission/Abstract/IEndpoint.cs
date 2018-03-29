using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Mission.Transport;
using Raven.Serializer;

namespace Raven.Mission.Abstract
{
    public interface IEndpoint:IDisposable
    {
        void UseMiddleWare(IMiddleWare middleWare,IDataSerializer serializer);
    }
}
