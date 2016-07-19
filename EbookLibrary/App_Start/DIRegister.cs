using EbookLibrary.Contracts;
using EbookLibrary.Gutenberg;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using lib_config = EbookLibrary.Configuration;

namespace EbookLibrary
{
    public static class DIRegister
    {
        public static void Register(HttpConfiguration config)
        {
            IUnityContainer container = new UnityContainer();
            container.LoadConfiguration();            

            config.DependencyResolver = new DependencyResolver(container);
        }
    }
}