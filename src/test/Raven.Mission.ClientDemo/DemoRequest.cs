using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Mission.Messages;

namespace Raven.Mission.ClientDemo
{
    public class DemoRequest:MissionMessage
    {
        public string OrderNo { get; set; }
    }
}
