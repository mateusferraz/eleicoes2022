using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessarBu
{
    public class Settings
    {
        public static IConfigurationRoot Configuration { get; set; }        

        public static string GetSetting (string name)
        {

            var builder = new ConfigurationBuilder()
                   .SetBasePath(Environment.CurrentDirectory)
                   .AddJsonFile("appsettings.json");//, optional: true, reloadOnChange: true);
            Configuration = builder.Build();            

            string setting = Configuration[name];
            return setting;
        }
    }

}
