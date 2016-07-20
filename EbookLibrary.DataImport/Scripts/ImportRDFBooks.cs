using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EbookLibrary.DataImport.Scripts
{
    [Script("books-rdf-import")]
    public class ImportRDFBooks : IScript
    {
        private IMongoDatabase db;
        private IMongoDatabase DB
        {
            get
            {
                if (db != null)
                {
                    return db;
                }
                else
                {
                    var client = new MongoClient(ConfigurationManager.AppSettings["mongo-connection-string"]);
                    DB = client.GetDatabase("ebooklibrary");
                    return DB;
                }
            }
            set
            {
                db = value;
            }
        }

        public async Task ExecuteAsync()
        {
            var data_folder = ConfigurationManager.AppSettings["data-directory"];
            var directories = Directory.GetDirectories(data_folder);

            Parallel.ForEach(directories, dir =>
            {
                var files = Directory.GetFiles(dir).ToList();
                if (files.Count() > 0)
                {
                    files.ForEach(x =>
                    {
                        XmlDocument doc = GetXMLDocument(x);
                        XmlNamespaceManager ns = GetNamespaceManager(doc);

                        var author = GetAuthor(doc, ns);
                        var title = GetTitle(doc, ns);
                        var book_id = GetBookId(doc, ns);
                        var category = GetBookCategory(doc, ns);

                        var book_collection = DB.GetCollection<BsonDocument>("ebooks");
                        var filter = new BsonDocument { { "book_id", book_id } };
                        var book_doc = book_collection.Find(filter).FirstOrDefault() ?? new BsonDocument();

                        book_doc.Add("book_id", book_id);
                        book_doc.Add("title", title);
                        book_doc.Add("author", author);
                        book_doc.Add("book_category", category);
                        book_collection.ReplaceOne(filter, book_doc, new UpdateOptions { IsUpsert = true });
                        Console.WriteLine(string.Format("Book {0} updated", title));
                    });
                }
            });
        }


        #region Private Methods
        private string GetTitle(XmlDocument doc, XmlNamespaceManager ns)
        {
            try
            {
                return doc.SelectSingleNode("//dcterms:title", ns).InnerText;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string GetAuthor(XmlDocument doc, XmlNamespaceManager ns)
        {
            try
            {
                return doc.SelectSingleNode("//dcterms:creator//pgterms:name", ns).InnerText;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string GetBookId(XmlDocument doc, XmlNamespaceManager ns)
        {
            try
            {
                var ebook = doc.SelectSingleNode("//pgterms:ebook", ns);
                var ebook_attrs = ebook.Attributes;
                var about_attr = ebook_attrs["rdf:about"];
                var book_id = about_attr.Value.Replace("ebooks/", "");

                return book_id;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string GetBookCategory(XmlDocument doc, XmlNamespaceManager ns)
        {
            try
            {
                var bookshelf = doc.SelectSingleNode("//pgterms:bookshelf//rdf:value", ns);
                if (bookshelf != null)
                {
                    var category_collection = DB.GetCollection<BsonDocument>("ebook_categories");
                    var filter = Builders<BsonDocument>.Filter.Regex("display_name", new BsonRegularExpression(bookshelf.InnerText, "i"));
                    var cat_doc = category_collection.Find(filter).FirstOrDefault();
                    return cat_doc["_id"].ToString();
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private XmlNamespaceManager GetNamespaceManager(XmlDocument doc)
        {
            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("dcterms", "http://purl.org/dc/terms/");
            ns.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            ns.AddNamespace("base", "http://www.gutenberg.org/");
            ns.AddNamespace("pgterms", "http://www.gutenberg.org/2009/pgterms/");
            ns.AddNamespace("dcam", "http://purl.org/dc/dcam/");
            ns.AddNamespace("cc", "http://web.resource.org/cc/");
            ns.AddNamespace("rdfs", "http://www.w3.org/2000/01/rdf-schema#");

            return ns;
        }

        private XmlDocument GetXMLDocument(string filepath)
        {
            var xml = File.ReadAllText(filepath);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc;
        }
        #endregion
    }
}
