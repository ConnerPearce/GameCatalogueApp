using Autofac;
using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes.StorageManager;
using GameCatalogueApp.Pages.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace GameCatalogueApp.Pages.Home
{
    // This is my home page
    // I use it to pass delegates throughout my entire program
    // Its also the page that displays the search bar when the program starts

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        // These are to pass through my EventHandlers
        public delegate void LoginFunction(object sender, EventArgs e);
        public delegate void UserFunction(object sender, EventArgs e);
        public delegate void GameList(object sender, SelectedItemChangedEventArgs e);
        
        // For my Error Messages
        public delegate void ErrorHandling(string error);

        // For Search functionality
        public delegate void Search(string message);

        // Creating local instances of the delegates to use in this page
        private readonly LoginFunction _loginFunction;
        private readonly UserFunction _userFunction;
        private readonly ErrorHandling _errorHandling;
        private readonly Search _search;
        private readonly GameList _gameList;

        // This is the home Page and will be loaded first
        public HomePage(LoginFunction loginFunction, UserFunction userFunction, Search search, ErrorHandling errorHandling, GameList gameList)
        {
            _loginFunction = loginFunction;
            _userFunction = userFunction;
            _errorHandling = errorHandling;
            _gameList = gameList;
            _search = search;


            InitializeComponent();
        }

        // When the app navigates to this page then it'll run this
        protected override void OnAppearing()
        {
            // Sets my event handlers for the .clicked function
            if (App.isLoggedIn)
            {
                btnLoginUser.Text = App.user.UName;
                btnLoginUser.Clicked += new EventHandler(_userFunction);
            }
            else
            {
                btnLoginUser.Text = "Login";
                btnLoginUser.Clicked += new EventHandler(_loginFunction);
            }
        }

        protected override void OnDisappearing()
        {
            // Removes the event handlers, allowing them to be reset again
            if (App.isLoggedIn)
                btnLoginUser.Clicked -= new EventHandler(_userFunction);
            else
                btnLoginUser.Clicked -= new EventHandler(_loginFunction);
        }

        private void searchBarGame_SearchButtonPressed(object sender, EventArgs e) 
        {
            // Runs the search function
            activityIndicator.IsRunning = true;
            _search(searchBarGame.Text);
            activityIndicator.IsRunning = false;
        }
    }
}