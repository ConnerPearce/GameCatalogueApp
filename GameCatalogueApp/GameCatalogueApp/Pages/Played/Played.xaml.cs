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

namespace GameCatalogueApp.Pages.Played
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Played : ContentPage
    {
        IContainer container;

        private readonly HomePage.UserFunction _userFunction;
        private readonly HomePage.ErrorHandling _errorHandling;
        private readonly HomePage.GameList _gameList;


        public Played(HomePage.UserFunction userFunction, HomePage.ErrorHandling errorHandling, HomePage.GameList gameList)
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
            if (container != null)
                container.Dispose();

            btnUser.Clicked -= new EventHandler(_userFunction);
            lstGames.ItemSelected -= new EventHandler<SelectedItemChangedEventArgs>(_gameList);
            lstGames.ItemsSource = null;
        }

        // Gets the information from the Played table in the database
        private async void PopulateInfo()
        {
            container = DependancyInjection.Configure();
            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IWishlistPlayedBackend>();
                if (App.isLoggedIn)
                {
                    var items = await app.GetPlayed(_errorHandling, App.user.Id);
                    if (items != null)
                        lstGames.ItemsSource = items;
                }
                else
                    _errorHandling("Please log in to view this");
            }
        }
      
    }
}