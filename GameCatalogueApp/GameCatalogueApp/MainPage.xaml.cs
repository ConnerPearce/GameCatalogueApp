using Autofac;
using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes.Pages.LoginPage;
using GameCatalogueApp.Classes.StorageManager;
using GameCatalogueApp.Pages.DetailedItem;
using GameCatalogueApp.Pages.Home;
using GameCatalogueApp.Pages.Login;
using GameCatalogueApp.Pages.Played;
using GameCatalogueApp.Pages.Search;
using GameCatalogueApp.Pages.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GameCatalogueApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        // This is used for my dependancy injection
        private Autofac.IContainer container = null;

        // These bools just keep track of running methods so they dont run more than once (i.e stops multiple button clicks)
        private bool isLogin;
        private bool isUser;
        private bool isWishlist;
        private bool isCompleted;

        // This is a dummy page (No UI, No Front End, instantly navigates away)
        // It contains all events/methods that are used more than once and arent unique throughout my program
        // This way i can make a localized space to store these so that i wont have to make 50 changes, i just need to make 1
        // The methods are passed around using Delegates
        public MainPage()
        {
            InitializeComponent();
        }

        // As soon as this page starts to be loaded it will navigate to my home page, passing all the neccessary stuff as it goes
        protected override async void OnAppearing()        
        {
            if (!activityInd.IsRunning)
            {
                activityInd.IsRunning = true;
                // Checks local storage for users information
                App.txtUsername = await Storage.ReadTextFileAsync(App.uNameLocation, DisplayError);
                App.txtPwrd = await Storage.ReadTextFileAsync(App.pwrdLocation, DisplayError);

                // Checks if the app wants to use the custom api or RAWG Api
                // Is stored locally as a string of either true or false
                // Converts it to a bool for simplicity sake as it cant be saved as anything less than a string
                App.useCustomAPI = Convert.ToBoolean(await Storage.ReadTextFileAsync(App.customApiLocation, DisplayError));

                // If the username and password are not empty (Details are being remembered) then it will run this to log the user in
                if (!string.IsNullOrEmpty(App.txtUsername) && !string.IsNullOrEmpty(App.txtPwrd))
                {
                    container = DependancyInjection.Configure();
                    using (var scope = container.BeginLifetimeScope())
                    {
                        var app = scope.Resolve<ILoginBackend>();

                        // If it runs into errors trying to log the user in then it'll display an alert, this is all handled automatically in my app using the DisplayError method which is a delegate
                        var person = await app.GetUser(App.txtUsername, App.txtPwrd, DisplayError);
                        if (person != null)
                        {
                            App.isLoggedIn = true;
                            App.user = person;
                        }
                    }
                }

                // Navigates to another page and passes through click events and some methods like error handling
                await Navigation.PushAsync(new HomePage(LoginButton_Click, UserButton_Click, Search, DisplayError, GetGamesFromlist_Selected));
            }            
        }

        // Called when leaving the page (Navigating away from)
        // Clears up containers and resets the activity Indicator if its still running
        protected override void OnDisappearing() 
        {
            if (container != null)
                container.Dispose();
            activityInd.IsRunning = false;
        }


        // DISPLAYS ERROR MESSAGES //

        // By passing this method through my program as a delegate i can manage all my errors using a generic handler that will display the error to the user
        private async void DisplayError(string error) => await DisplayAlert("Something went wrong", $"Error info: {error}", "Ok");

        // LOGIN BUTTON //

        // Navigates to Login Page
        // Limits button clicks (Wont do anything if its already trying to do something)
        private async void LoginButton_Click(object sender, EventArgs e)
        {
            if (!isLogin)
            {
                isLogin = true;
                await Navigation.PushAsync(new Login(DisplayError));
                isLogin = false;
            }
        }

        // USER SETTINGS BUTTON //

        // Navigates to Settings Page
        // Limits button clicks (Wont do anything if its already trying to do something)
        private async void UserButton_Click(object sender, EventArgs e) 
        {
            if (!isUser)
            {
                isUser = true;
                await Navigation.PushAsync(new Settings(DisplayError, WishlistButton_Click, CompletedButton_Click));
                isUser = false;
            }
        }

        // WISHLIST NAVIGATION BUTTON //

        // Navigates to the wishlist
        // Limits button clicks (Wont do anything if its already trying to do something)
        private async void WishlistButton_Click(object sender, EventArgs e)
        {
            if (!isWishlist)
            {
                isWishlist = true;
                await Navigation.PushAsync(new Pages.Wishlist.WishlistPage(UserButton_Click, DisplayError, GetGamesFromlist_Selected));
                isWishlist = false;
            }
        }

        // COMPLETED NAVIGATION BUTTON //

        // Navigates to the completed games page
        // Limits button clicks (Wont do anything if its already trying to do something)
        private async void CompletedButton_Click(object sender, EventArgs e) 
        {
            if (!isCompleted)
            {
                isCompleted = true;
                await Navigation.PushAsync(new Pages.Played.Played(UserButton_Click, DisplayError, GetGamesFromlist_Selected));
                isCompleted = false;
            }
        }


        // HANDLES SEARCHING // 

        // Takes in a message, verifys it, then sends it to the Search Page, displays an error if the search is empty
        // (Generally xamarin doesnt allow it to be empty by default but best to check anyway as good practice)
        private async void Search(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                await Navigation.PushAsync(new Search(message, LoginButton_Click, UserButton_Click, DisplayError));
            }
            else
                DisplayError("Please enter something to search for");
        }

        // HANDLES ITEM SELECTION

        // Grabs item from listview then navigates to new page
        // If their is an error then it will display the exception that was thrown (Errors can happen when converting e.SelectedItem to IGame so this is a precaution to allow the user to report an error)
        private async void GetGamesFromlist_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var item = (IGame)e.SelectedItem; // Gets the item from the list and pushes it onto the detailed page
                await Navigation.PushAsync(new DetailedPage(item.id, LoginButton_Click, UserButton_Click, DisplayError));
            }
            catch (Exception ex)
            {
                DisplayError($"Unable to select this item\nAdditional Info: {ex.Message}");
            }
        }
    }
}
