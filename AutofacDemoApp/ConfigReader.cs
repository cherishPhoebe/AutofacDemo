using System;
using System.Collections.Generic;
using System.Text;

namespace AutofacDemoApp
{
    public class ConfigReader:IConfigReader
    {

        public ConfigReader(string configSectionName) {
            Console.WriteLine(configSectionName);
        }
    }
}
