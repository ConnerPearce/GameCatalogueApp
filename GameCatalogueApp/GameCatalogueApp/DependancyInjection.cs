using Autofac;
using GameCatalogueApp.API;
using GameCatalogueApp.API.Data;
using GameCatalogueApp.Classes;
using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes._Custom_API.Proxys;
using GameCatalogueApp.Classes.API.Data;
using GameCatalogueApp.Classes.ConnectionManager;
using GameCatalogueApp.Classes.Pages.DetailedPage;
using GameCatalogueApp.Classes.Pages.LoginPage;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using static GameCatalogueApp.API.GameProxy;

namespace GameCatalogueApp
{
    // This class manages dependancy injection by using a NuGet called AutoFac
    // The documentation can be found at https://autofaccn.readthedocs.io/en/latest/getting-started/index.html
    public static class DependancyInjection
    {
        // This method contains all variable injections
        // It also manages All classes and returns them as their Interface Counterparts where needed
        // It can also instantiate variables at build time
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            // DATA CLASSES //

            // Existing API data
            builder.RegisterType<GameRootObject>().As<IGameRootObject>();
            builder.RegisterType<SingleGameRootObject>().As<ISingleGameRootObject>();

            // Custom API Data
            builder.RegisterType<User>().As<IUser>();
            builder.RegisterType<Played>().As<IPlayed>();
            builder.RegisterType<Game>().As<IGame>();
            builder.RegisterType<Wishlist>().As<IWishlist>();

            // HELPER CLASSES //
            builder.RegisterType<CheckConnection>().As<ICheckConnection>();

            // PAGE BACKENDS //
            builder.RegisterType<SearchBackend>().As<ISearchBackend>();
            builder.RegisterType<DetailedPageBackend>().As<IDetailedPageBackend>();
            builder.RegisterType<LoginBackend>().As<ILoginBackend>();

            // API Classes //
            string baseAddress = "https://api.rawg.io/api/";
            builder.Register<GameProxy>((c, p) =>
            {
                return new GameProxy(baseAddress);
            }).As<IGameProxy>();

            string customAPIbaseAddress = "http://localhost:5000/";
            builder.Register<UserProxy>((c, p) =>
            {
                return new UserProxy(customAPIbaseAddress);
            }).As<IUserProxy>();


            return builder.Build();
        }       
    }
}
