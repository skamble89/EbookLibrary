using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbookLibrary.Models
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PropertyAttribute : Attribute
    {
        public PropertyAttribute(string fieldName)
        {
            this.FieldName = fieldName;
        }

        public string FieldName { get; set; }
    }
}
