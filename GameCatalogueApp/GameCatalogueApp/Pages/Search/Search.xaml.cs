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
        private readonly string _searchItem;
        public Search(string searchItem)
        {
            InitializeComponent();
            searchItem = _searchItem;
            searchBarGame.Text = _searchItem;
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