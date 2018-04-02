using Raven.Mission.Messages;

namespace Raven.Mission.TestApi
{
    public class DemoRequest:IMissionMessage
    {
        public string OrderNo { get; set; }
        public int MissionId { get; set; }
        public string ReplyQueue { get; set; }
    }
}
