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
            popupLoginView.IsVisible = true;
            lblWelcomeMessage.IsVisible = false;
            txtSearchBar.IsVisible = false;
        }

        // LOGIN POP UP
        // The Registration label is clickable
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            // Navigate to registration page
            await Navigation.PushAsync(new Registration.Registration());
        }
    }
}