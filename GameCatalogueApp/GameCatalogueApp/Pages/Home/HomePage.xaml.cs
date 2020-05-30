﻿using Autofac;
using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Pages.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameCatalogueApp.Pages.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public static bool isLoggedIn = false;
        public static IUser user = new User();


        public HomePage()
        {
            InitializeComponent();
            if (!isLoggedIn)
            {
                btnUser.IsVisible = false;
                btnLogin.IsVisible = true;
            }
            else
            {
                btnLogin.IsVisible = false;
                btnUser.Text = user.UName;
                btnUser.IsVisible = true;
            }
        }

        //LOGIN BUTTON
        private async void btnLogin_Clicked(object sender, EventArgs e) => await Navigation.PushAsync(new Login.Login());
        //USER SETTINGS BUTTON
        private async void btnUser_Clicked(object sender, EventArgs e) => await Navigation.PushAsync(new Settings.Settings());


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