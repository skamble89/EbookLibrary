using EbookLibrary.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbookLibrary.DataImport
{
    public static class EntityExtensions
    {
        public static void Save<T>(this T obj) where T : IDBEntity
        {
            var client = new MongoClient(ConfigurationManager.AppSettings["mongo-connection-string"]);
            var db = client.GetDatabase("ebooklibrary");

            var type = typeof(T);
            var attrs = type.GetCustomAttributes(false).ToList();
            var schemaAttr = attrs.FirstOrDefault(x => (x as SchemaAttribute) != null) as SchemaAttribute;
            
            if (schemaAttr != null)
            {
                
            }
        }
    }
}
