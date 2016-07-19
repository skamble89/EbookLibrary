using EbookLibrary.Models;
using EbookLibrary.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Practices.Unity;

namespace EbookLibrary.Controllers
{
    [RoutePrefix("api/library")]
    public class LibraryController : ApiController
    {
        private ILibrary _library;

        [InjectionConstructor]
        public LibraryController(ILibrary library)
        {
            _library = library;
        }

        [HttpGet]
        [Route("libraries")]
        public List<Library> GetLibraries()
        {
            return new List<Library>();
        }

        [HttpGet]
        [Route("{libraryId}/categories")]
        public List<Category> GetCategories(string libraryId)
        {
            return _library.GetCategories();
        }

        [HttpGet]
        [Route("{libraryId}/{categoryId}/books")]
        public List<Book> GetBooks(string libraryId, string categoryId)
        {
            return _library.GetBooks(categoryId);
        }

        [HttpGet]
        [Route("books/{searchTerm}")]
        public List<Book> SearchBooks(string searchTerm)
        {
            return _library.SearchBooks(searchTerm);
        }

        [HttpGet]
        [Route("{libraryId}/{categoryId}/{bookId}")]
        public Book GetBook(string libraryId, string categoryId, string bookId)
        {
            return _library.GetBook(categoryId, bookId);
        }
    }
}
