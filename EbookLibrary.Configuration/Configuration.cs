using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbookLibrary.Configuration
{
    public static class Configuration
    {
        private static LibrarySection config = ConfigurationManager.GetSection("ebooklibrary.config") as LibrarySection;

        public static LibraryCollection Libraries
        {
            get
            {
                return config.Libraries;
            }
        }
    }
}
