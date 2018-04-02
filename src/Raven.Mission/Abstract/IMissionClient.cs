using System.Threading.Tasks;
using Raven.Mission.Messages;

namespace Raven.Mission.Abstract
{
    public interface IMissionClient:IEndpoint
    {
        /// <summary>
        /// 执行客户端请求，获取队列响应
        /// </summary>
        /// <typeparam name="TResult">返回结果类型</typeparam>
        /// <typeparam name="TRequest">请求参数类型</typeparam>
        /// <param name="client"></param>
        /// <param name="resource"></param>
        /// <param name="request"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<TResult> ExcuteAsync<TResult, TRequest>(string resource, TRequest request,int timeout) where TRequest : IMissionMessage;
        /// <summary>
        /// 开始监听响应消息
        /// </summary>
        void StartSubscribe();
        /// <summary>
        /// 使用的http请求客户端
        /// </summary>
        /// <param name="host"></param>
        void UseHttpClient(string host);
    }
}
