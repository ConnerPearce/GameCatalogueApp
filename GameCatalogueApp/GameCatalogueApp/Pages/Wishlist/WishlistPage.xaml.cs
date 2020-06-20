﻿using Autofac;
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
    // This page is for my Wishlist
    // It manages displaying items on the wishlist

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WishlistPage : ContentPage
    {
        // This variable is used for my dependancy injection
        IContainer container;

        // These are creating an instance of my Delegates from my HomePage.Xaml.cs

        // This is my user button click even handler
        private readonly HomePage.UserFunction _userFunction;
        // This is the delegate for my error handling method
        private readonly HomePage.ErrorHandling _errorHandling;

        // This is for selecting items from my wishlist and pushing the info onto the DetailedItem page
        private readonly HomePage.GameList _gameList;

        public WishlistPage(HomePage.UserFunction userFunction, HomePage.ErrorHandling errorHandling, HomePage.GameList gameList)
        {
            _userFunction = userFunction;
            _errorHandling = errorHandling;
            _gameList = gameList;

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            // Assigns event handlers to the button and list
            btnUser.Clicked += new EventHandler(_userFunction);
            lstGames.ItemSelected += new EventHandler<SelectedItemChangedEventArgs>(_gameList);

            // Assigns the username to be the text for the button
            btnUser.Text = App.user.UName;
            PopulateInfo();
        }


        protected override void OnDisappearing()
        {
            // Removes the event handles upon leaving the page
            btnUser.Clicked -= new EventHandler(_userFunction);
            lstGames.ItemSelected -= new EventHandler<SelectedItemChangedEventArgs>(_gameList);
            // Clears the lists item source
            lstGames.ItemsSource = null;
            
            if(container != null)
                container.Dispose();
        }

        // Gets the information from the Wishlist
        private async void PopulateInfo()
        {
            container = DependancyInjection.Configure();
            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IWishlistPlayedBackend>();
                if (App.isLoggedIn) // If the user isnt logged in there wouldnt be anything to display (They shouldnt be able to enter this page anyway but better safe then sorry)
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