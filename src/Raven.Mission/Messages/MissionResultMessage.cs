using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Mission.Messages
{
    public class MissionResultMessage
    {
        public string MissionId { get; set; }

        public object Result { get; set; }
    }
}
