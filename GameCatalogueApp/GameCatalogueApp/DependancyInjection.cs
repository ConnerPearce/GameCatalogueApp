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
using GameCatalogueApp.Classes.Pages.RegistrationPage;
using GameCatalogueApp.Classes.Pages.SettingsPage;
using GameCatalogueApp.Classes.Pages.WishlistPlayedPage;
using GameCatalogueApp.Classes.StorageManager;
using GameCatalogueApp.Pages.Home;
using GameCatalogueApp.Pages.Settings;
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

        // These variables are the URL's for my two API's
        // They are injected during build time to different parts of my app to be used
        // This way i can keep the URL's in one place for easy access to change
        private static readonly string customAPIbaseAddress = "https://gamecatalog.api.labnet.nz/";
        private static readonly string baseAddress = "https://api.rawg.io/api/";


        // By registering all my classes and having an interface i can use this to inject items during build time rather than run time, increasing speed while running
        // By calling this Configure method it will return an IContainer that includes all the types for my entire program
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
            builder.RegisterType<RegistrationBackend>().As<IRegistrationBackend>();
            builder.RegisterType<SettingsBackend>().As<ISettingsBackend>();
            builder.RegisterType<WishlistPlayedBackend>().As<IWishlistPlayedBackend>();

            // By specifying a return i can inject specific variables into the constructors such as the base address for my api

            // API CLASSES //
            builder.Register<GameProxy>((c, p) =>
            {
                return new GameProxy(baseAddress);
            }).As<IGameProxy>();


            // CUSTOM API CLASSES //
            builder.Register<UserProxy>((c, p) =>
            {
                return new UserProxy(customAPIbaseAddress);
            }).As<IUserProxy>();

            builder.Register<CustomGameProxy>((c, p) =>
            {    
               return new CustomGameProxy(customAPIbaseAddress);
            }).As<ICustomGameProxy>();

            builder.Register<WishlistPlayedProxy>((c, p) =>
            {
                return new WishlistPlayedProxy(customAPIbaseAddress);
            }).As<IWishlistPlayedProxy>();

            return builder.Build();
        }       
    }
}
