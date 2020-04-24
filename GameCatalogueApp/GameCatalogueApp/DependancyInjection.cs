﻿using Autofac;
using GameCatalogueApp.API;
using GameCatalogueApp.API.Data;
using GameCatalogueApp.Classes;
using GameCatalogueApp.Classes.API.Data;
using GameCatalogueApp.Classes.ConnectionManager;
using GameCatalogueApp.Classes.Pages.DetailedPage;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameCatalogueApp
{
    // This class manages dependancy intjection
    public static class DependancyInjection
    {
        // This method contains all variable injections
        // It also manages All classes and returns them as their Interface Counterparts where needed
        // It can also instantiate variables at build time
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            // Data Classes
            builder.RegisterType<GameRootObject>().As<IGameRootObject>();
            builder.RegisterType<SingleGameRootObject>().As<ISingleGameRootObject>();

            // Helper Classes
            builder.RegisterType<CheckConnection>().As<ICheckConnection>();

            // Page Backends
            builder.RegisterType<SearchBackend>().As<ISearchBackend>();
            builder.RegisterType<DetailedPageBackend>().As<IDetailedPageBackend>();

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
