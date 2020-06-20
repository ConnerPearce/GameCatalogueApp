using Autofac;
using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes.Pages.SettingsPage;
using GameCatalogueApp.Classes.StorageManager;
using GameCatalogueApp.Pages.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameCatalogueApp.Pages.Settings
{
   // This page is my settings page
   // It handles all local settings as well as updating user info

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        // Since im pushing straight from my main page to here i need to respecify the delegates
        public delegate void WishlistFunction(object sender, EventArgs e);
        public delegate void CompletedFunction(object sender, EventArgs e);

        private IContainer container;

        // This is the delegate that handles all my errors
        private readonly HomePage.ErrorHandling _displayError;

        // Below is for both my button clicks to navigate to the wishlist and completed page
        private readonly WishlistFunction _wishlistFunction;
        private readonly CompletedFunction _completedFunction;

        public Settings(HomePage.ErrorHandling displayError, WishlistFunction wishlistFunction, CompletedFunction completedFunction)
        {
            _displayError = displayError;
            _wishlistFunction = wishlistFunction;
            _completedFunction = completedFunction;

            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            // Changes scRemember to be on if remember me is selected and stored locally
            // Shorthand for multiple if statements checking if await Storage.ReadTextFileAsync(App.detailsLocation, _displayError) is equal to "true", if it is then it will return true, if not then it will return false
            scRemeber.On = await Storage.ReadTextFileAsync(App.detailsLocation, _displayError) == "true" ? true : false;

            // If the user wants to use the custom API this is true otherwise they will use the RAWG Api
            scMutliSearch.On = App.useCustomAPI;
            if (App.isLoggedIn)
            {
                // Displays user info for updating (Unable to show if the user isnt logged in)
                tblAccountSettings.IsVisible = true;
                tblsAccount.BindingContext = App.user;
            }
            else // Hides the account settings section if the user isnt logged in
                tblAccountSettings.IsVisible = false;

            // Assigns clicked event handlers
            btnCompleted.Clicked += new EventHandler(_completedFunction);
            btnWishlist.Clicked += new EventHandler(_wishlistFunction);

        }

        protected override async void OnDisappearing()
        {
            // Sets options upon leaving the page
            App.useCustomAPI = scMutliSearch.On;

            // Saves the wether or not to use the custom API or Rawg API
            // (App.useCustomAPI ? "true" : "false") is just short hand for an if statement saying if app.useCustomAPI is true then "true" will be passed else it will pass "false"
            await Storage.WriteTextFileAsync(App.customApiLocation, App.useCustomAPI ? "true" : "false", _displayError);

            // Removes event handlers
            btnCompleted.Clicked -= new EventHandler(_completedFunction);
            btnWishlist.Clicked -= new EventHandler(_wishlistFunction);
            if(container != null)
                container.Dispose();
        }

        // Used to update User Info
        private void btnSubmitChanges_Clicked(object sender, EventArgs e) => ChangeUserInfo();

        // Used to logout
        private void btnLogout_Clicked(object sender, EventArgs e) => Logout();

        // Used to remember user login details
        private void scRemeber_OnChanged(object sender, ToggledEventArgs e) => RememberMe();

        // Linked to the submitChanges button
        private async void ChangeUserInfo()
        {
            // Validates info entered if it isnt empty
            // If it is then it will display a appropriete error 
            if (string.IsNullOrEmpty(txtUname.Text))
                _displayError("Please enter a username");
            else if (string.IsNullOrEmpty(txtFName.Text))
                _displayError("Please enter your first name");
            else if (string.IsNullOrEmpty(txtLName.Text))
                _displayError("Please enter your last name");
            else if (string.IsNullOrEmpty(txtEmail.Text))
                _displayError("Please enter your email");
            else if (string.IsNullOrEmpty(txtPwrd.Text))
                _displayError("Please enter a password");
            else
            {
                // Informs the user their account is being updated
                await DisplayAlert("Progress", "Updating Account Now", "Ok");
                container = DependancyInjection.Configure();
                using (var scope = container.BeginLifetimeScope())
                {
                    var app = scope.Resolve<ISettingsBackend>();
                    var success = await app.UpdateUser(new User
                    {
                        Id = App.user.Id,
                        UName = txtUname.Text,
                        Email = txtEmail.Text,
                        FName = txtFName.Text,
                        LName = txtLName.Text,
                        Pwrd = txtPwrd.Text
                    }, _displayError);

                    // If they user was updated successfully
                    if (success)
                        await DisplayAlert("Updated!", "Your details have been updated!", "Ok");
                }
            }
        }

        // Linked to the Logout button
        private async void Logout()
        {
            // Checks if the user is sure they want to logout and assigns their answer to a bool
            bool accept = await DisplayAlert("Confirmation", "Are you sure you want to logout?", "Yes", "No");

            if (accept)
            {
                // Resets all of the local storage items (When you log out your no longer remembered)
                await Storage.WriteTextFileAsync(App.uNameLocation, "", _displayError);
                await Storage.WriteTextFileAsync(App.pwrdLocation, "", _displayError);
                await Storage.WriteTextFileAsync(App.customApiLocation, "false", _displayError);
                await Storage.WriteTextFileAsync(App.detailsLocation, "false", _displayError);

                // Switches all options to false to clear up things
                // Since you can no longer switch back to Rawg API if you are on the custom one
                // This way if they are logged out then they can use the more robust api as a general thing rather than the custom one that can add to wishlist etc
                scMutliSearch.On = false;
                scRemeber.On = false;

                // Resets all App. variables to remove all user info
                App.useCustomAPI = false;
                App.user = new User();
                App.isLoggedIn = false;

                // Returns to main page and reloads the app with no user info anymore
                await Navigation.PushAsync(new MainPage());
            }
        }

        // Linked to the Remember Details Switch
        private async void RememberMe()
        {
            bool remember = scRemeber.On;

            // Sets the remember me information in local storage 
            await Storage.WriteTextFileAsync(App.detailsLocation, remember ? "true" : "false", _displayError);

            if (remember)
            {
                // Remembering details then saves the username and password
                await Storage.WriteTextFileAsync(App.uNameLocation, App.user.UName, _displayError);
                await Storage.WriteTextFileAsync(App.pwrdLocation, App.user.Pwrd, _displayError);
            }
            else
            {
                // Turning remember off then removes the details
                await Storage.WriteTextFileAsync(App.uNameLocation, "", _displayError);
                await Storage.WriteTextFileAsync(App.pwrdLocation, "", _displayError);
            }

        }
    }
}