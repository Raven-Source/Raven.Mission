using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Raven.Mission.Abstract;
using Raven.Mission.Messages;
using Raven.Mission.Transport;
using Raven.Serializer;

namespace Raven.Mission.Server
{
    /// <summary>
    /// 服务端
    /// </summary>
    internal  class MissionServer:IMissionServer
    {
        private  IMiddleWare _queue;
        private  IDataSerializer _serializer;

        
        /// <summary>
        /// 异步执行任务
        /// </summary>
        /// <typeparam name="TMessage">真实的响应类型</typeparam>
        /// <param name="request"></param>
        /// <param name="mission"></param>
        public  void AsyncExecuteMission<TMessage>(MissionMessage request, Task<TMessage> mission) 
        {
            if (_queue == null || _serializer == null)
            {
                throw new Exception("请先注入中间件及序列化器");
            }
            Action<Task<TMessage>> a = async (t) =>
            {
                TMessage message = await t;
                var msg = new MissionResultMessage<TMessage>
                {
                    MissionId = request.MissionId,
                    Result = message
                };
                var msgBytes = _serializer.Serialize(msg);
                var body = new byte[4 + msgBytes.Length];
                var idBytes = BitConverter.GetBytes(request.MissionId);
                Array.Copy(idBytes,body,4);
                Array.Copy(msgBytes,0,body,4,msgBytes.Length);
                await _queue.PublishAsync(request.ReplyQueue, body);
            };
            mission.ContinueWith(a).ConfigureAwait(false);
        }

        public  void Dispose()
        {
            _queue.StopAsync().Wait();
        }


        public void UseMiddleWare(IMiddleWare middleWare,IDataSerializer serializer)
        {
            _queue = middleWare;
            _serializer = serializer;
        }
    }
}
