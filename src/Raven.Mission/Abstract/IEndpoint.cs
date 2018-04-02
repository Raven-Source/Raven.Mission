using System;
using Raven.Mission.Transport;
using Raven.Serializer;

namespace Raven.Mission.Abstract
{
    public interface IEndpoint:IDisposable
    {
        void UseMiddleWare(IMiddleWare middleWare,IDataSerializer serializer,ILogger logger=null);
    }
}
