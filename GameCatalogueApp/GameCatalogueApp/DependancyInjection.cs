using Autofac;
using GameCatalogueApp.API;
using GameCatalogueApp.API.Data;
using GameCatalogueApp.Classes;
using GameCatalogueApp.Classes.ConnectionManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameCatalogueApp
{
    // This class manages dependancy intjection
    public static class DependancyInjection
    {
        public static int RootObject { get; private set; }

        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            // Data Classes
            builder.RegisterType<GameRootObject>().As<IGameRootObject>();

            // Helper Classes
            builder.RegisterType<CheckConnection>().As<ICheckConnection>();

            // Page Backends
            builder.RegisterType<SearchBackend>().As<ISearchBackend>();

            // API Classes
            string baseAddress = "https://api.rawg.io/api/";
            builder.Register<GameProxy>((c, p) =>
            {
                return new GameProxy(baseAddress);
            }).As<IGameProxy>();


            return builder.Build();
        }

    }
}
