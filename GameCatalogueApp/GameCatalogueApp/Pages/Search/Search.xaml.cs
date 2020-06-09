using Autofac;
using GameCatalogueApp.API.Data;
using GameCatalogueApp.Classes;
using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Pages.DetailedItem;
using GameCatalogueApp.Pages.Home;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace GameCatalogueApp.Pages.Search
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Search : ContentPage
    {
        // Creating local instances of the delegates to use in this page
        private HomePage.LoginFunction _loginFunction;
        private HomePage.UserFunction _userFunction;
        private HomePage.ErrorHandling _errorHandling;

        private IContainer container;
        private readonly string _searchItem;

        public Search(string searchItem, HomePage.LoginFunction loginFunction, HomePage.UserFunction userFunction, HomePage.ErrorHandling errorHandling)
        {
            _searchItem = searchItem;
            _loginFunction = loginFunction;
            _userFunction = userFunction;
            _errorHandling = errorHandling;
            InitializeComponent();
        }

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
            searchBarGame.Text = _searchItem;
            ItemSearch(_searchItem);
        }

        protected override void OnDisappearing()
        {
            // Removes the event handlers, allowing them to be reset again
            if (App.isLoggedIn)
                btnLoginUser.Clicked -= new EventHandler(_userFunction);
            else
                btnLoginUser.Clicked -= new EventHandler(_loginFunction);

            if(container != null)
                container.Dispose();
        }

        // Begins Dependancy Injection for all asosciated Classes
        // Uses AutoFac for dependancy injection
        // NEEDS TO BE CALLED FOR EACH PAGE
        private async void ItemSearch(string search)
        {
            activityIndicator.IsRunning = true;
            container = DependancyInjection.Configure();
            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ISearchBackend>();
                if (App.useCustomAPI)
                {
                    var games = await app.GetCustomGames(search, _errorHandling);
                    if (games != null)
                        lstGames.ItemsSource = games;
                }
                else
                {
                    var games = await app.GetGames(search, _errorHandling);
                    if (games != null && games.results != null)
                        lstGames.ItemsSource = games.results;
                }               
                activityIndicator.IsRunning = false;
            }
        }

        private void searchBarGame_SearchButtonPressed(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(searchBarGame.Text))
                ItemSearch(searchBarGame.Text);
            else
                _errorHandling("Enter a game to search for");
        }

        private async void lstGames_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (App.useCustomAPI) // if its using the custom API then it handles the selected item differently
                {
                    var item = (Game)e.SelectedItem;
                    await Navigation.PushAsync(new DetailedPage(item.id, _loginFunction, _userFunction, _errorHandling));
                }
                else
                {
                    var item = (Result)e.SelectedItem;
                    await Navigation.PushAsync(new DetailedPage(item.slug, _loginFunction, _userFunction, _errorHandling));
                }

            }
            catch (Exception ex) // General exception handling, more of to show the error and allow the program to continue running rather than handle it
            {
                _errorHandling($"Unable to select this item\nAdditional Info: {ex.Message}");
            }
        }
    }
}