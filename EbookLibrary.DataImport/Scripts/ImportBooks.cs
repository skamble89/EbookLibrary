using EbookLibrary.Gutenberg;
using EbookLibrary.Models;
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
    [Script("books-import")]
    public class ImportBooks : IScript
    {
        public async Task ExecuteAsync()
        {
            var gutenberg = new GutenbergLibrary();
            var client = new MongoClient(ConfigurationManager.AppSettings["mongo-connection-string"]);
            var db = client.GetDatabase("ebooklibrary");
            var category_collection = db.GetCollection<BsonDocument>("ebook_categories");
            var categories = category_collection
                                    .Find(_ => true)
                                    .ToListAsync()
                                    .Result
                                    .Select(x =>
                                    {
                                        return new Category
                                        {
                                            Name = x["display_name"].ToString(),
                                            Url = x["category_key"].ToString()
                                        };
                                    }).ToList();

            categories.ForEach((category) =>
            {
                var books = gutenberg.GetBooks(category.Url);
                books.ForEach((book) =>
                {
                    var book_details = gutenberg.GetBook(category.Url, book.Id);
                    var book_collection = db.GetCollection<BsonDocument>("ebooks");
                    var filter = new BsonDocument { { "book_id", book.Id } };
                    var book_doc = book_collection.Find(filter).FirstOrDefault() ?? new BsonDocument();

                    book_doc.Add("book_id", book.Id);
                    book_doc.Add("title", book.Title);
                    book_doc.Add("author", book_details.Author);
                    book_doc.Add("book_category", category.Url);
                    book_collection.ReplaceOne(filter, book_doc, new UpdateOptions { IsUpsert = true });
                    Console.WriteLine(string.Format("Book {0} updated", book.Title));
                    Task.Delay(5000);
                });
            });
        }
    }
}
