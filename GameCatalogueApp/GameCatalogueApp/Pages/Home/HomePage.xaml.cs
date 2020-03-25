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

        /// LOGIN FUNCTIONS ///

        //LOGIN BUTTON
        private void btnLogin_Clicked(object sender, EventArgs e)
        {
            // Shows the popup while making other content not visible
            // An Issue came up where things would overlap which could not be fixed without using custom renderers
            // Or custom Programming (Less generic program)
            ChangeVisibility();
        }

        // LOGIN POP UP
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            // Navigate to registration page
            ChangeVisibility();
            await Navigation.PushAsync(new Registration.Registration());
        }

        private void StackLayout_Tapped(object sender, EventArgs e)
        {
            ChangeVisibility();
        }       

        private async void searchBarGame_SearchButtonPressed(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new Search.Search(searchBarGame.Text));
        }

        private void ChangeVisibility()
        {
            if (popupLoginView.IsVisible)
            {
                popupLoginView.IsVisible = false;
                lblWelcomeMessage.IsVisible = true;
                searchBarGame.IsVisible = true;
            }
            else
            {
                popupLoginView.IsVisible = true;
                lblWelcomeMessage.IsVisible = false;
                searchBarGame.IsVisible = false;
            }
        }
    }
}