using System;
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
    internal class MissionServer : IMissionServer
    {
        private IMiddleWare _queue;
        private IDataSerializer _serializer;
        private ILogger _logger;

        /// <summary>
        /// 异步执行任务
        /// </summary>
        /// <typeparam name="TMessage">真实的响应类型</typeparam>
        /// <param name="request"></param>
        /// <param name="mission"></param>
        public void AsyncExecuteMission<TMessage>(IMissionMessage request, Task<TMessage> mission)
        {
            if (_queue == null || _serializer == null)
            {
                throw new Exception("请先注入中间件及序列化器");
            }
            Action<Task<TMessage>> a = async (t) =>
            {
                try
                {
                    TMessage message = await t;
                    var msgBytes = _serializer.Serialize(message);
                    var body = new byte[4 + msgBytes.Length];
                    var idBytes = BitConverter.GetBytes(request.MissionId);
                    Array.Copy(idBytes, body, 4);
                    Array.Copy(msgBytes, 0, body, 4, msgBytes.Length);
                    await _queue.PublishAsync(request.ReplyQueue, body);
                }
                catch (Exception e)
                {
                    _logger?.LogError(e,null);
                }
            };
            mission.ContinueWith(a).ConfigureAwait(false);
        }

        public void Dispose()
        {
            _queue.StopAsync().Wait();
        }


        public void UseMiddleWare(IMiddleWare middleWare, IDataSerializer serializer, ILogger logger = null)
        {
            _queue = middleWare;
            _serializer = serializer;
            _logger = logger;
        }
    }
}
