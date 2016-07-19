using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace EbookLibrary.Configuration
{
    public class LibrarySection : ConfigurationSection
    {
        [ConfigurationProperty("libraries")]
        public LibraryCollection Libraries
        {
            get
            {
                return (LibraryCollection)this["libraries"];
            }            
        }
    }
}
