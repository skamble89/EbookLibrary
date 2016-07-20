using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbookLibrary.Models
{
    [Schema("ebook_categories")]
    public class Category : IDBEntity
    {
        [Property("category_key")]
        public string Url { get; set; }

        [Property("display_name")]
        public string Name { get; set; }
    }
}