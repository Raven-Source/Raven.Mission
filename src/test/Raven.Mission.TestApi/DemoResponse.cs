using MessagePack;

namespace Raven.Mission.TestApi
{
    [MessagePackObject]
    public class DemoResponse
    {
        [Key(0)]
        public string Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
    }
}
