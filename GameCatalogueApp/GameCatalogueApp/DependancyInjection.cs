using Autofac;
using GameCatalogueApp.API;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameCatalogueApp
{
    // This class manages dependancy intjection
    public static class DependancyInjection
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();


            string baseAddress = "https://api.rawg.io/api/";
            builder.Register<GameProxy>((c, p) =>
            {
                return new GameProxy(baseAddress);
            }).As<IGameProxy>();


            return builder.Build();
        }

    }
}
