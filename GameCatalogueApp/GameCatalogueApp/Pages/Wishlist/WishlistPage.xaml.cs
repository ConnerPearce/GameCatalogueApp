using Autofac;
using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes.Pages.WishlistPlayedPage;
using GameCatalogueApp.Pages.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameCatalogueApp.Pages.Wishlist
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WishlistPage : ContentPage
    {
        IContainer container;
        private HomePage.UserFunction _userFunction;
        private HomePage.ErrorHandling _errorHandling;
        private HomePage.GameList _gameList;

        public WishlistPage(HomePage.UserFunction userFunction, HomePage.ErrorHandling errorHandling, HomePage.GameList gameList)
        {
            _userFunction = userFunction;
            _errorHandling = errorHandling;
            _gameList = gameList;

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            btnUser.Clicked += new EventHandler(_userFunction);
            lstGames.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(_gameList);
            btnUser.Text = App.user.UName;
            PopulateInfo();
        }


        protected override void OnDisappearing()
        {
            btnUser.Clicked -= new EventHandler(_userFunction);
            lstGames.ItemSelected -= new EventHandler<SelectedItemChangedEventArgs>(_gameList);
            lstGames.ItemsSource = null;
            
            if(container != null)
                container.Dispose();
        }

        private async void PopulateInfo()
        {
            container = DependancyInjection.Configure();
            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IWishlistPlayedBackend>();
                if (App.isLoggedIn) // If the user isnt logged in there wouldnt be anything to display
                {
                    var items = await app.GetWishlist(_errorHandling, App.user.Id);
                    if (items != null)
                        lstGames.ItemsSource = items;
                }
                else
                    _errorHandling("Please log in to view this");
            }
        }
   
    }
}