using Raven.Mission.Transport;
using System;
using System.Threading.Tasks;

namespace Raven.Mission
{
    /// <summary>
    /// 任务指挥官
    /// </summary>
    public class Commander
    {
        /// <summary>
        /// 
        /// </summary>
        private IMiddleWare middleWare;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="middleWare"></param>
        public Commander(IMiddleWare middleWare)
        {
            this.middleWare = middleWare;
        }

        /// <summary>
        /// 异步执行任务
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="missionId"></param>
        /// <param name="mission"></param>
        public void AsyncExecuteMission<TMessage>(string missionId, Task<TMessage> mission)
        {
            Action<Task<TMessage>> a = async (t) =>
            {
                TMessage message = await t;
                await middleWare.PublishAsync(missionId, message);
            };

            mission.ContinueWith(a);
        }

        /// <summary>
        /// 异步执行任务
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="missionId"></param>
        /// <param name="mission"></param>
        /// <param name="taskContinue"></param>
        public void AsyncExecuteMission<TMessage>(string missionId, Task<TMessage> mission, out Task taskContinue)
        {
            Action<Task<TMessage>> a = async (t) =>
            {
                TMessage message = await t;
                await middleWare.PublishAsync(missionId, message); 
            };

            taskContinue = mission.ContinueWith(a);
        }

        /// <summary>
        /// 异步接收任务的完成结果
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="missionId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<TMessage> AsyncReceiveMissionCompleteResult<TMessage>(string missionId, int timeout = 60000)
        {
            TaskCompletionSource<TMessage> taskCompletion = new TaskCompletionSource<TMessage>();

            await middleWare.SubscribeAsync<TMessage>(missionId, msg => taskCompletion.TrySetResult(msg)).ConfigureAwait(false);

            try
            {
                return await taskCompletion.Task.WithTimeout(TimeSpan.FromMilliseconds(timeout)).ConfigureAwait(false);
            }
            catch (TimeoutException)
            {
                return await Task.FromResult(default(TMessage));
            }
            finally
            {
                await middleWare.UnsubscribeAsync(missionId).ConfigureAwait(false);
            }
        }



    }
}
