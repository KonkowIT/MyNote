using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Utils
{
    internal class SettingsRetriever
    {
        internal ConnectionStringSettings GetConnectionString(string name)
        {
            ConnectionStringSettings correctSettings = null;
            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;

            foreach (ConnectionStringSettings s in settings)
            {
                if (s.Name == name)
                {
                    correctSettings = s;
                }   
            }

            return correctSettings;
        }
    }
}
