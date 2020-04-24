using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameCatalogueApp.Pages.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        //LOGIN BUTTON
        private async void btnLogin_Clicked(object sender, EventArgs e) => await Navigation.PushAsync(new Login.Login());

        private async void searchBarGame_SearchButtonPressed(object sender, EventArgs e)
        {
            string message = searchBarGame.Text;
            if (!string.IsNullOrEmpty(message))
            {
                activityIndicator.IsRunning = true;
                await Navigation.PushAsync(new Search.Search(message));
                activityIndicator.IsRunning = false;
            }
            else
                await DisplayAlert("Something went wrong", $"Error info: Enter a game to search for", "Ok");
        }
    }
}