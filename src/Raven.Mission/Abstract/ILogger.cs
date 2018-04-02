using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Mission.Abstract
{
    public interface ILogger
    {
        void Error(Exception ex);
        void Error(string error);
    }
}
