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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public delegate void WishlistFunction(object sender, EventArgs e);
        public delegate void CompletedFunction(object sender, EventArgs e);

        private IContainer container;

        private readonly HomePage.ErrorHandling _displayError;
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
            // Sets the switch to be on if rememberDetails.txt returns "true" in text, if its anything else it will set it to off
            // By Having it in this method it will check each tim the page is loaded in case its changed, rather than checking it once when the program loads and not changing it 
            scRemeber.On = (await Storage.ReadTextFileAsync(App.detailsLocation, _displayError) == "true") ? true : false;
            scMutliSearch.On = App.useCustomAPI;
            if (App.isLoggedIn)
            {
                tblAccountSettings.IsVisible = true;
                tblsAccount.BindingContext = App.user;
            }
            else
                tblAccountSettings.IsVisible = false;
            btnCompleted.Clicked += new EventHandler(_completedFunction);
            btnWishlist.Clicked += new EventHandler(_wishlistFunction);

        }

        protected override async void OnDisappearing()
        {
            App.useCustomAPI = scMutliSearch.On;
            await Storage.WriteTextFileAsync(App.customApiLocation, App.useCustomAPI ? "true" : "false", _displayError);

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

                    if (success)
                        await DisplayAlert("Updated!", "Your details have been updated!", "Ok");
                }
            }
        }

        // Linked to the Logout button
        private async void Logout()
        {
            bool accept = await DisplayAlert("Confirmation", "Are you sure you want to logout?", "Yes", "No");
            if (accept)
            {
                // Resets all of the local storage items (When you log out your no longer remembered)
                await Storage.WriteTextFileAsync(App.uNameLocation, "", _displayError);
                await Storage.WriteTextFileAsync(App.pwrdLocation, "", _displayError);
                await Storage.WriteTextFileAsync(App.customApiLocation, "false", _displayError);
                await Storage.WriteTextFileAsync(App.detailsLocation, "false", _displayError);

                // Switches all options to false to clear up things
                scMutliSearch.On = false;
                scRemeber.On = false;

                // Resets all App. variables to remove all user info
                App.useCustomAPI = false;
                App.user = new User();
                App.isLoggedIn = false;
                App.txtUsername = string.Empty;
                App.txtPwrd = string.Empty;

                await Navigation.PushAsync(new MainPage());
            }
        }

        // Linked to the Remember Details Switch
        private async void RememberMe()
        {
            bool remember = scRemeber.On;
            var option = remember ? "true" : "false";

            await Storage.WriteTextFileAsync(App.detailsLocation, option, _displayError);

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