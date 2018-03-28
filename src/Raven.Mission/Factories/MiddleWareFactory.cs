using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raven.Mission.Factories
{
    public  class MiddleWareFactory
    {

        private MiddleWareFactory()
        {
        }

        static MiddleWareFactory(){
            Instance = new MiddleWareFactory();
        }

        public static MiddleWareFactory Instance { get; }
    }
}
