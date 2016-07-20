using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbookLibrary.Models
{
    [Schema("ebooks")]
    public class Book : IDBEntity
    {
        [Property("book_id")]
        public string Id { get; set; }

        [Property("title")]
        public string Title { get; set; }

        [Property("author")]
        public string Author { get; set; }

        [Property("category")]
        public string Category { get; set; }
    }
}