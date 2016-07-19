using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbookLibrary.Configuration
{
    [ConfigurationCollection(typeof(Library), AddItemName = "library")]
    public class LibraryCollection : ConfigurationElementCollection
    {
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Library)element).Name;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Library();
        }

        new public Library this[string name]
        {
            get { return (Library)BaseGet(name); }
        }

        new public Library this[int index]
        {
            get { return (Library)BaseGet(index); }
        }
    }
}
