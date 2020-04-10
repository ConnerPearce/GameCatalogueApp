using Autofac;
using GameCatalogueApp.API.Data;
using GameCatalogueApp.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameCatalogueApp.Pages.Search
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Search : ContentPage
    {
        private IContainer container;
        public Search(string searchItem)
        {
            InitializeComponent();
            searchBarGame.Text = searchItem;
            ItemSearch(searchItem);
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
                var games = await app.GetGames(search, DisplayError);
                if (games != null && games.results != null)
                {
                    lstGames.ItemsSource = games.results;
                    activityIndicator.IsRunning = false;
                }
                activityIndicator.IsRunning = false;

            }
        }

        // The Method for Alert Displays that will be passed using delegates
        // HANDLES ALL ERROR MESSAGES //
        private async void DisplayError(string error)
        {
            await DisplayAlert("Something went wrong", $"Error info: {error}", "Ok");
        }

        private void StackLayout_Tapped(object sender, EventArgs e)
        {
            ChangeVisibility();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            // Navigate to registration page
            ChangeVisibility();
            await Navigation.PushAsync(new Registration.Registration());
        }

        private void searchBarGame_SearchButtonPressed(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(searchBarGame.Text))
            {
                ItemSearch(searchBarGame.Text);
            }
            else
                DisplayError("Enter a game to search for");
        }

        private void btnLogin_Clicked(object sender, EventArgs e)
        {
            ChangeVisibility();
        }

        private void ChangeVisibility()
        {
            if (popupLoginView.IsVisible)
            {
                popupLoginView.IsVisible = false;
                searchBarGame.IsVisible = true;
            }
            else
            {
                popupLoginView.IsVisible = true;
                searchBarGame.IsVisible = false;
            }
        }

        private async void lstGames_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushAsync(new DetailedItem.DetailedPage());
        }
    }
}