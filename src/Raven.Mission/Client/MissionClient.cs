using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Raven.Mission.Abstract;
using Raven.Mission.Messages;
using Raven.Mission.Transport;
using Raven.Serializer;

namespace Raven.Mission.Client
{
    internal class MissionClient : IMissionClient
    {
        /// <summary>
        /// 结果task字典 缓存消息请求的响应结果类型，响应结果任务，key为消息id
        /// </summary>
        private readonly ConcurrentDictionary<int, Tuple<Type, TaskCompletionSource<object>>> _resultDic =
            new ConcurrentDictionary<int, Tuple<Type, TaskCompletionSource<object>>>();

        private string _queueId;
        private volatile bool _subscribed = false;
        private readonly object _lockObj = new object();
        private readonly object _msgIdlock = new object();
        private IMiddleWare _messagequeue;
        private IDataSerializer _serializer;
        private int _msgId = 0;
        private IHttpClient _client;
        private ILogger _logger;

        public async Task<TResult> ExcuteAsync<TResult, TRequest>(string resource, TRequest request, int timeout)
            where TRequest : IMissionMessage
        {

            if (_messagequeue == null || _serializer == null || _client == null)
            {
                throw new Exception("请先注入中间件、序列化器及Http请求客户端");
            }

            if (!_subscribed)
            {
                throw new Exception("请先执行IMissionClient.StartSubscribe()方法");
            }
            object result;
            var id = Interlocked.Increment(ref _msgId);
            try
            {
                ResetMsgId();
                request.ReplyQueue = _queueId;

                request.MissionId = id;
                var task = new TaskCompletionSource<object>();
                _resultDic.TryAdd(request.MissionId,
                    new Tuple<Type, TaskCompletionSource<object>>(typeof(TResult), task));
                //发送请求
                await _client.SendAsync(resource, request);
                // return default(TResult);
                //等待响应
                if (timeout > 0)
                {
                    result = await task.Task.WithTimeout(TimeSpan.FromSeconds(timeout)).ConfigureAwait(false);
                }
                else
                {
                    result = await task.Task.ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return default(TResult);
            }
            finally
            {
                _resultDic.TryRemove(id, out var task);
            }


            return (TResult)result;
        }

        public void StartSubscribe()
        {
            _queueId = Guid.NewGuid().ToString("N");
            if (!_subscribed)
            {
                lock (_lockObj)
                {
                    if (!_subscribed)
                    {
                        _messagequeue.SubscribeAsync<byte[]>(_queueId, OnRecived).Wait();
                        _subscribed = true;
                    }
                }
            }
        }

        public void UseHttpClient(string host)
        {
            _client = new HttpClientWrapper(host);
        }

        /// <summary>
        /// 收到响应消息时，设置请求Task的结果
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool OnRecived(byte[] msg)
        {
            var id = BitConverter.ToInt32(msg, 0);
            var msgBytes = new byte[msg.Length - 4];
            Array.Copy(msg, 4, msgBytes, 0, msgBytes.Length);
            if (_resultDic.TryRemove(id, out var task))
            {
                var type = task.Item1;
                var result = _serializer.Deserialize(type, msgBytes);
                task.Item2.SetResult(result);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 重置消息ID,一次进程周期内请求次数达到Int最大值，则需要重置
        /// 基本不会出现这种情况
        /// </summary>
        private void ResetMsgId()
        {
            if (_msgId == Int32.MaxValue)
            {
                lock (_msgIdlock)
                {
                    if (_msgId == Int32.MaxValue)
                    {
                        _msgId = 0;
                    }
                }
            }
        }



        public void Dispose()
        {
            _messagequeue.UnsubscribeAsync(_queueId).Wait();
            _client?.Dispose();
        }

        public void UseMiddleWare(IMiddleWare middleWare, IDataSerializer serializer, ILogger logger = null)
        {
            _messagequeue = middleWare;
            _serializer = serializer;
            _logger = logger;
        }
    }
}
