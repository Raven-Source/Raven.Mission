namespace Raven.Mission.Messages
{
    public interface IMissionMessage
    {
        int MissionId { get; set; }
        string ReplyQueue { get; set; }
    }
    
}
