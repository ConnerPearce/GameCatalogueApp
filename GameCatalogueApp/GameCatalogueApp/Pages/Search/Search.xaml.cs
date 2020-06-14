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
        private readonly HomePage.LoginFunction _loginFunction;
        private readonly HomePage.UserFunction _userFunction;
        private readonly HomePage.ErrorHandling _errorHandling;

        private IContainer container;
        private readonly string _searchItem;
        private ObservableCollection<Game> games = new ObservableCollection<Game>();
        private ObservableCollection<Result> results = new ObservableCollection<Result>();

        private List<Game> allGames = new List<Game>();
        private List<Result> allResults = new List<Result>();

        // Used to set upper and lower bounds of how many items should be displayed
        private int maxItems;
        private int minItems;

        public Search(string searchItem, HomePage.LoginFunction loginFunction, HomePage.UserFunction userFunction, HomePage.ErrorHandling errorHandling)
        {
            _searchItem = searchItem;
            _loginFunction = loginFunction;
            _userFunction = userFunction;
            _errorHandling = errorHandling;
            InitializeComponent();
        }

        private void ClearInfo()
        {
            // Reset the list and upper and lower bound
            minItems = 0;
            maxItems = 10;
            if (allGames != null && allGames.Count > 0)
                allGames.Clear();
            if(allResults != null && allResults.Count > 0)
                allResults.Clear();

            games = new ObservableCollection<Game>();
            results = new ObservableCollection<Result>();
        }

        protected override void OnAppearing()
        {
            if (!activityIndicator.IsRunning)
            {
                activityIndicator.IsRunning = true;
                maxItems = 10;
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
            
        }

        protected override void OnDisappearing()
        {
            ClearInfo();

            // Removes the event handlers, allowing them to be reset again
            if (App.isLoggedIn)
                btnLoginUser.Clicked -= new EventHandler(_userFunction);
            else
                btnLoginUser.Clicked -= new EventHandler(_loginFunction);

            if(container != null)
                container.Dispose();
        }

        private void lstGames_ItemAppearing(object sender, ItemVisibilityEventArgs e) => LoadMoreGames(e);
        private void lstGames_ItemSelected(object sender, SelectedItemChangedEventArgs e) => GetGameFromList(e);
        private async void searchBarGame_SearchButtonPressed(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(searchBarGame.Text))
            {
                ClearInfo();
                activityIndicator.IsRunning = true;
                await Navigation.PushAsync(new Search(searchBarGame.Text, _loginFunction, _userFunction, _errorHandling));
                activityIndicator.IsRunning = false;
            }
            else
                _errorHandling("Enter a game to search for");
        }

        // Begins Dependancy Injection for all asosciated Classes
        // Uses AutoFac for dependancy injection
        // NEEDS TO BE CALLED FOR EACH PAGE
        private async void ItemSearch(string search)
        {
            container = DependancyInjection.Configure();
            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ISearchBackend>();
                if (App.useCustomAPI)
                {
                    allGames = await app.GetCustomGames(search, _errorHandling);
                    if (allGames != null)
                    {
                        if (allGames.Count >= 10)
                        {
                            for (int i = minItems; i < maxItems; i++)
                            {
                                games.Add(allGames[i]);
                            }
                        }
                        else
                        {
                            foreach (var item in allGames)
                            {
                                games.Add(item);
                            }
                        }

                        lstGames.ItemsSource = games;

                    }
                }
                else
                {
                    var temp = await app.GetGames(search, _errorHandling);
                    allResults = temp.results;
                    if (allResults != null)
                    {
                        if (allResults.Count >= 10)
                        {
                            for (int i = minItems; i < maxItems; i++)
                            {
                                results.Add(allResults[i]);
                            }
                        }
                        else
                        {
                            foreach (var item in allResults)
                            {
                                results.Add(item);
                            }
                        }


                        lstGames.ItemsSource = results;
                    }
                }               
                activityIndicator.IsRunning = false;
            }
        }

        private async void GetGameFromList(SelectedItemChangedEventArgs e)
        {
            try
            {
                activityIndicator.IsRunning = true;
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
                activityIndicator.IsRunning = false;

            }
            catch (Exception ex) // General exception handling, more of to show the error and allow the program to continue running rather than handle it
            {
                _errorHandling($"Unable to select this item\nAdditional Info: {ex.Message}");
            }
        }


        private void LoadMoreGames(ItemVisibilityEventArgs e)
        {
            if (App.useCustomAPI)
            {
                if (activityIndicator.IsRunning || games.Count == 0)
                    return;
                if ((IGame)e.Item == games[games.Count - 1])
                {
                    activityIndicator.IsRunning = true;

                    maxItems += 10;
                    minItems += 10;

                    for (int i = minItems; i < maxItems && i < allGames.Count; i++)
                    {
                        games.Add(allGames[i]);
                    }

                    activityIndicator.IsRunning = false;
                }
            }
            else
            {
                if (activityIndicator.IsRunning || results.Count == 0)
                    return;

                if ((Result)e.Item == results[results.Count - 1])
                {
                    activityIndicator.IsRunning = true;

                    maxItems += 10;
                    minItems += 10;

                    for (int i = minItems; i < maxItems && i < allResults.Count; i++)
                    {
                        results.Add(allResults[i]);
                    }

                    activityIndicator.IsRunning = false;
                }
            }
        }
    }
}