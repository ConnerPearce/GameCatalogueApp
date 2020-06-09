using Autofac;
using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes.Pages.LoginPage;
using GameCatalogueApp.Classes.StorageManager;
using GameCatalogueApp.Pages.Home;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameCatalogueApp
{
    public partial class App : Application
    {
        public static bool isLoggedIn;
        public static bool useCustomAPI = true;

        public static string txtUsername;
        public static string txtPwrd;

        public static IUser user = new User();

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }

    }
}
