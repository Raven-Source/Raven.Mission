using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Mission.Messages
{
    /// <summary>
    /// 请求参数
    /// </summary>
    public class MissionMessage
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public int MissionId { get; set; }
        /// <summary>
        /// 响应队列名称
        /// </summary>
        public string ReplyQueue { get; set; }
    }
}
