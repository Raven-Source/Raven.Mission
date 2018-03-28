using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Mission.Messages
{
    public class MissionResultMessage<T>:MissionMessage
    {
        /// <summary>
        /// 响应内容 json字符串
        /// </summary>
        public T Result { get; set; }
    }
}
