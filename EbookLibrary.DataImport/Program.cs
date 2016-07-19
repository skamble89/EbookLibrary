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
using System.Reflection;
using EbookLibrary.DataImport.Scripts;

namespace EbookLibrary.DataImport
{
    class Program
    {
        static void Main(string[] args)
        {
            ScriptRunner runner = new ScriptRunner();
            Task.Run(async () =>
            {
                await runner.RunScript("books-import");
            }).Wait();
        }
    }
}
