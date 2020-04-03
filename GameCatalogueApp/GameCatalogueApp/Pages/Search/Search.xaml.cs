using Autofac;
using GameCatalogueApp.Classes;
using System;
using System.Collections.Generic;
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
        private string _searchItem;
        private IContainer container;
        public Search(string searchItem)
        {
            InitializeComponent();
            _searchItem = searchItem;            
            searchBarGame.Text = _searchItem;
            activityIndicator.IsRunning = true;
            StartSearch();
        }

        public async void StartSearch()
        {
            container = DependancyInjection.Configure();
            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<ISearchBackend>();
                var games = await app.GetGames(_searchItem, DisplayError);
                if (games != null)
                {
                    lstGames.ItemsSource = games.results;
                    activityIndicator.IsRunning = false;
                }

            }
        }

        private async void DisplayError(string error)
        {
            await DisplayAlert("Uh Oh!", $"Error info: {error}", "Ok");
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
    }
}