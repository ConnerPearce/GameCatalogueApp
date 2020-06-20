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
    // This is my search page
    // It manages searching for games in both of the API's im using as well as limiting items displayed

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Search : ContentPage
    {
        // These are creating an instance of my Delegates from my HomePage.Xaml.cs
        private readonly HomePage.LoginFunction _loginFunction;
        private readonly HomePage.UserFunction _userFunction;
        private readonly HomePage.ErrorHandling _errorHandling;

        private IContainer container;

        // the Search item, its readonly as it shouldnt be changed once its in this class
        private readonly string _searchItem;

        // These are what i use to bind to the front end
        // games is used for my Custom API and results are used for the RAWG Api
        // ObserveableCollections update dynamically so if you were to add something new to them it would display the new item without reloading the entire page
        private ObservableCollection<Game> games = new ObservableCollection<Game>();
        private ObservableCollection<Result> results = new ObservableCollection<Result>();

        // This list is temporary storage so i can limit the amount of items being shown at any given time
        // The lists will contain all the games or results then i will later limit the items shown
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

        // Used to clear the info of the page and reset variables
        private void ClearInfo()
        {
            // Reset the list and upper and lower bound
            minItems = 0;
            maxItems = 10;
            // If the items arent null and there is items in the observeable collection it should clear all the items
            if (allGames != null && allGames.Count > 0)
                allGames.Clear();
            if(allResults != null && allResults.Count > 0)
                allResults.Clear();

            //  Resets the observeable collections
            games = new ObservableCollection<Game>();
            results = new ObservableCollection<Result>();
        }

        // Called when navigating to the page
        protected override void OnAppearing()
        {
            // Sets the upper bounds of the amount of items to be shown
            maxItems = 10;

            // Sets my event handlers for the .clicked function
            // If the user is logged in then it assigns a different event handler than if the user is not logged in
            // It also assigns different text to the button
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

            // Sets the search bar text in the top so you can see the item you are currently searching for
            searchBarGame.Text = _searchItem;
            ItemSearch(_searchItem);            
        }

        // Called when leaving the page
        protected override void OnDisappearing()
        {
            // Resets the user info
            ClearInfo();

            // Removes the event handlers, allowing them to be reset again
            if (App.isLoggedIn)
                btnLoginUser.Clicked -= new EventHandler(_userFunction);
            else
                btnLoginUser.Clicked -= new EventHandler(_loginFunction);

            // Clears up dependancy injection resources
            if(container != null)
                container.Dispose();
        }

        private void lstGames_ItemAppearing(object sender, ItemVisibilityEventArgs e) => LoadMoreGames(e);
        private void lstGames_ItemSelected(object sender, SelectedItemChangedEventArgs e) => GetGameFromList(e);

        private async void searchBarGame_SearchButtonPressed(object sender, EventArgs e)
        {
            // If the search bar isnt empty it will push a new search page to search again
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

        // Used to search for items
        // Searches through different databases depending on what option is chosen
        private async void ItemSearch(string search)
        {
            if (!activityIndicator.IsRunning)
            {
                activityIndicator.IsRunning = true;
                container = DependancyInjection.Configure();
                using (var scope = container.BeginLifetimeScope())
                {
                    var app = scope.Resolve<ISearchBackend>();
                    if (App.useCustomAPI)
                    {
                        // gets all games that match the search
                        allGames = await app.GetCustomGames(search, _errorHandling);
                        if (allGames != null)
                        {
                            // Limits how many items are displayed at a time so the user can load more by scrolling down
                            if (allGames.Count >= 10)
                            {
                                // If their is more than 10 items then it will scroll through all items and add them to the Observeable collection
                                // Grabing them out based on index
                                for (int i = minItems; i < maxItems; i++)
                                {
                                    games.Add(allGames[i]);
                                }
                            }
                            else
                            {
                                // If there is less than 10 items in the list it will grab all items
                                // As 10 is the minimum amount of items to be shown it would crash if their was less than 10 items
                                // This will handle it in the case of less than 10 items
                                foreach (var item in allGames)
                                {
                                    games.Add(item);
                                }
                            }

                            // Sets the item source of the ListView to display the info
                            lstGames.ItemsSource = games;
                        }
                    }
                    else
                    {
                        // Rawg Api returns an IGameRootObject 
                        var temp = await app.GetGames(search, _errorHandling);
                        allResults = temp.results;
                        if (allResults != null)
                        {
                            // Limits how many items are displayed at a time so the user can load more by scrolling down
                            if (allResults.Count >= 10)
                            {
                                // If their is more than 10 items then it will scroll through all items and add them to the Observeable collection
                                // Grabing them out based on index
                                for (int i = minItems; i < maxItems; i++)
                                {
                                    results.Add(allResults[i]);
                                }
                            }
                            else
                            {
                                // If there is less than 10 items in the list it will grab all items
                                // As 10 is the minimum amount of items to be shown it would crash if their was less than 10 items
                                // This will handle it in the case of less than 10 items
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
        }

        // Gets the item from the list then pushes the unique ID to the game and pushes it to the 
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

        // Used to load more items when the bottom of the list is reached
        // Checks every item that loads in to check if you are the bottom of the list
        private void LoadMoreGames(ItemVisibilityEventArgs e)
        {
            if (App.useCustomAPI)
            {
                // if the method is already running or the games is 0 it shouldnt do anything
                if (activityIndicator.IsRunning || games.Count == 0)
                    return;
                // if the current item appearing is the last item in the list then run the code
                if ((IGame)e.Item == games[games.Count - 1])
                {
                    activityIndicator.IsRunning = true;

                    // Increment the max and min items to filter through the next set of items and add 10 more items
                    maxItems += 10;
                    minItems += 10;

                    // It will scroll through and add the next 10 items of the list while i is less than the amount of games retrieved 
                    //(So it will stop if it runs out of games to place (i.e there is 9 items left but it needs 10))
                    for (int i = minItems; i < maxItems && i < allGames.Count; i++)
                    {
                        games.Add(allGames[i]);
                    }

                    activityIndicator.IsRunning = false;
                }
            }
            else
            {
                // if the method is already running or the games is 0 it shouldnt do anything
                if (activityIndicator.IsRunning || results.Count == 0)
                    return;

                // if the current item appearing is the last item in the list then run the code
                if ((Result)e.Item == results[results.Count - 1])
                {
                    activityIndicator.IsRunning = true;

                    // Increment the max and min items to filter through the next set of items and add 10 more items
                    maxItems += 10;
                    minItems += 10;

                    // It will scroll through and add the next 10 items of the list while i is less than the amount of games retrieved 
                    //(So it will stop if it runs out of games to place (i.e there is 9 items left but it needs 10))
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