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

        // These are all static variables i use through out my entire app

        //These bools help manage certain aspects like if the user is logged in etc
        public static bool isLoggedIn;
        public static bool useCustomAPI;

        // These are strings that indicate file names for certain items (Used for local storage to find files)
        public static readonly string uNameLocation = "username.txt";
        public static readonly string pwrdLocation = "password.txt";
        public static readonly string detailsLocation = "rememberDetails.txt";
        public static readonly string customApiLocation = "custom.txt";

        // This variable stores all my users information temporaraly
        // By keeping the user info centralized in one place it makes it easier to manage their information rather than having 50 copys of it you would have to clear when logging out
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
