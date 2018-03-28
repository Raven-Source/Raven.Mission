using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Raven.Mission.Messages;
using Raven.Mission.Transport;
using Raven.Serializer;

namespace Raven.Mission.Client
{
    public class MissionClient
    {
        /// <summary>
        /// 结果task字典 缓存消息请求的响应结果类型，响应结果任务，key为消息id
        /// </summary>
        private readonly ConcurrentDictionary<int, Tuple<Type, TaskCompletionSource<MissionMessage>>> _resultDic =
            new ConcurrentDictionary<int, Tuple<Type, TaskCompletionSource<MissionMessage>>>();

        private readonly string _queueId;
        private volatile bool _subscribed = false;
        private readonly object _lockObj = new object();

        private readonly IMiddleWare _messagequeue;
        private readonly IDataSerializer _serializer;
        private int _msgId=0;
        public MissionClient(IMiddleWare queue, IDataSerializer serializer)
        {
            //todo generator queue_id
            _queueId = Guid.NewGuid().ToString("N");
            _messagequeue = queue;
            _serializer = serializer;
            if (!_subscribed)
            {
                lock (_lockObj)
                {
                    if (!_subscribed)
                    {
                        //subscribe
                        _messagequeue.SubscribeAsync<byte[]>(_queueId, OnRecived).Wait();
                        _subscribed = true;
                    }
                }
            }
        }

        public async Task<TResult> ExcuteAsync<TResult, TRequest>(IHttpClient client, string resource, TRequest request, int timeout)
            where TRequest : MissionMessage
        {
            request.ReplyQueue = _queueId;
            request.MissionId = Interlocked.Increment(ref _msgId);
            var task = new TaskCompletionSource<MissionMessage>();
            _resultDic.TryAdd(request.MissionId,
                new Tuple<Type, TaskCompletionSource<MissionMessage>>(typeof(MissionResultMessage<TResult>), task));
            //发送请求
            await client.SendAsync(resource, request);
            MissionMessage result;
            //等待响应
            if (timeout > 0)
            {
                result = await task.Task.WithTimeout(TimeSpan.FromSeconds(timeout)).ConfigureAwait(false);
            }
            else
            {
                result = await task.Task.ConfigureAwait(false);
            }

            return ((MissionResultMessage<TResult>)result).Result;
        }

        /// <summary>
        /// 收到响应消息时，设置请求Task的结果
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool OnRecived(byte[] msg)
        {
            var id = BitConverter.ToInt32(msg, 0);
            var msgBytes = new byte[msg.Length-4];
            Array.Copy(msg,4,msgBytes,0,msgBytes.Length);
           // var message = _serializer.Deserialize<MissionMessage>(msg);
            if (_resultDic.TryRemove(id, out var task))
            {
                var type = task.Item1;
                var result = (MissionMessage)_serializer.Deserialize(type, msgBytes);
                task.Item2.SetResult(result);
                return true;
            }
            return false;
        }

        public async Task StopAsync()
        {
            await _messagequeue.UnsubscribeAsync(_queueId).ConfigureAwait(false);
        }


    }
}
