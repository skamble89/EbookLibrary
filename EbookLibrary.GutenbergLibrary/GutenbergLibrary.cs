using EbookLibrary.Contracts;
using EbookLibrary.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbookLibrary.Gutenberg
{
    public class GutenbergLibrary : ILibrary
    {
        private const string __gutenberg_base_url = "http://www.gutenberg.org/";

        public List<Category> GetCategories()
        {
            var result = new List<Category>();
            var client = new HtmlWeb();
            var doc = client.Load(__gutenberg_base_url + "wiki/Category:Bookshelf");
            var next_pages = doc.DocumentNode.SelectNodes("//a[@title='Category:Bookshelf']").GroupBy(x => x.Attributes["href"].Value).Select(x => x.First());

            result.AddRange(ParseCategories(doc));
            foreach (var page in next_pages)
            {
                doc = client.Load(HtmlEntity.DeEntitize("http:" + page.Attributes["href"].Value));
                result.AddRange(ParseCategories(doc));
            }

            return result;
        }

        public List<Book> GetBooks(string categoryId)
        {
            var books = new List<Book>();
            var client = new HtmlWeb();
            var doc = client.Load(__gutenberg_base_url + "wiki/" + categoryId);

            var book_links = doc.DocumentNode.SelectNodes("//li/a[@class='extiw']");
            foreach (var link in book_links)
            {
                books.Add(new Book
                {
                    Id = link.Attributes["href"].Value.Replace("//www.gutenberg.org/ebooks/", ""),
                    Title = link.InnerText
                });
            }
            return books;
        }

        public List<Book> SearchBooks(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public Book GetBook(string categoryId, string bookId)
        {
            var client = new HtmlWeb();
            var doc = client.Load(__gutenberg_base_url + "ebooks/" + bookId);
            var details_table = doc.DocumentNode.SelectSingleNode("//table[@class='bibrec']");

            var headline = details_table.SelectSingleNode("//*[@itemprop='headline']");
            var creator = details_table.SelectSingleNode("//*[@itemprop='creator']");

            return new Book
            {
                Id = bookId + ".txt.utf-8",
                Title = headline != null ? headline.InnerText.Trim() : string.Empty,
                Author = creator != null ? creator.InnerText.Trim() : string.Empty
            };
        }

        #region Private methods
        private List<Category> ParseCategories(HtmlDocument doc)
        {
            var result = new List<Category>();
            var bookshelf_container = doc.DocumentNode.SelectSingleNode("//div[@id='mw-pages']");
            var bookshelf = bookshelf_container.SelectNodes("div[@class='mw-content-ltr']//a");

            foreach (var node in bookshelf)
            {
                result.Add(new Category
                {
                    Url = node.Attributes["href"].Value.Replace("/wiki/", ""),
                    Name = node.InnerText
                });
            }

            return result;
        }
        #endregion
    }
}
