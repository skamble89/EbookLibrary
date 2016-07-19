using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbookLibrary.DataImport
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ScriptAttribute : Attribute
    {
        public ScriptAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
