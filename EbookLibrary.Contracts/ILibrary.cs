using EbookLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbookLibrary.Contracts
{
    public interface ILibrary
    {
        List<Category> GetCategories();

        List<Book> GetBooks(string categoryId);

        List<Book> SearchBooks(string searchTerm);

        Book GetBook(string categoryId, string bookId);
    }
}
