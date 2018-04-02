using Raven.Serializer;

namespace Raven.Mission.Abstract
{
    public interface IMissionConfig
    {
        /// <summary>
        /// 服务端域名
        /// </summary>
        string ServerHost { get; set; }
        /// <summary>
        /// 序列化器类型
        /// </summary>
        SerializerType SerializerType { get; set; }
    }
}
