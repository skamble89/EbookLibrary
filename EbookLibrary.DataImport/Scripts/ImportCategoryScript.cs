using EbookLibrary.Gutenberg;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbookLibrary.DataImport.Scripts
{
    [Script("category-import")]
    public class ImportCategoryScript : IScript
    {
        public async Task ExecuteAsync()
        {
            var gutenberg = new GutenbergLibrary();
            var categories = gutenberg.GetCategories();

            Parallel.ForEach(categories, (x) =>
            {
                var client = new MongoClient(ConfigurationManager.AppSettings["mongo-connection-string"]);
                var db = client.GetDatabase("ebooklibrary");
                var category_collection = db.GetCollection<BsonDocument>("ebook_categories");
                var filter = new BsonDocument { { "category_key", x.Url } };
                var category_doc = category_collection.Find(filter).FirstOrDefault() ?? new BsonDocument();

                category_doc.Add("display_name", x.Name);
                category_doc.Add("category_key", x.Url);
                category_collection.ReplaceOne(filter, category_doc, new UpdateOptions { IsUpsert = true });
                Console.WriteLine(string.Format("Category {0} updated", x.Name));
            });
        }
    }
}
