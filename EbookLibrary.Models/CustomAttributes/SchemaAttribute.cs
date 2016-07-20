using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbookLibrary.Models
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SchemaAttribute : Attribute
    {
        public SchemaAttribute(string schemaName)
        {
            this.SchemaName = schemaName;
        }

        public string SchemaName { get; set; }
    }
}
