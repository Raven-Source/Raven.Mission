using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if !NET451
using Microsoft.Extensions.Configuration;
#endif
using Raven.Mission.Abstract;

namespace Raven.Mission
{
    public static class ConfigUtils
    {

        public static T GetSection<T>(string sectionName) where T: class ,IMissionConfig ,new()
        {
#if NET451
            var config=ConfigurationManager.GetSection(sectionName);
            return (T) new T().GetBySection(config);
#else
            var configuration = new ConfigurationBuilder().AddJsonFile("app.json").Build();
            var section= configuration.GetSection(sectionName);
            return (T)(new T().GetBySection(section));
#endif
        }
    }
}
