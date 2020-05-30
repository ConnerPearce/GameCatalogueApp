using Autofac;
using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes.Pages.SettingsPage;
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
        private IUser user { get; set; }
        private IContainer container;

        public Settings()
        {
            InitializeComponent();
            PopulateTables();
        }
        private void PopulateTables()
        {
            if (HomePage.isLoggedIn)
            {
                tblAccountSettings.IsVisible = true;
                user = HomePage.user;
                tblsAccount.BindingContext = user;
            }
            else
                tblAccountSettings.IsVisible = false;
        }
        private async void DisplayError(string error) => await DisplayAlert("Something went wrong", $"Error info: {error}", "Ok");


        private async void btnWishlist_Clicked(object sender, EventArgs e) => await Navigation.PushAsync(new Wishlist.Wishlist());

        private async void btnCompleted_Clicked(object sender, EventArgs e) => await Navigation.PushAsync(new Played.Played());

        private void btnSubmitChanges_Clicked(object sender, EventArgs e) => ChangeUserInfo();

        private async void ChangeUserInfo()
        {
            if (string.IsNullOrEmpty(txtUname.Text))
                DisplayError("Please enter a username");

            else if (string.IsNullOrEmpty(txtFName.Text))
                DisplayError("Please enter your first name");
            else if (string.IsNullOrEmpty(txtLName.Text))
                DisplayError("Please enter your last name");
            else if (string.IsNullOrEmpty(txtEmail.Text))
                DisplayError("Please enter your email");
            else if (string.IsNullOrEmpty(txtPwrd.Text))
                DisplayError("Please enter a password");
            else
            {
                await DisplayAlert("Progress", "Updating Account Now", "Ok");
                container = DependancyInjection.Configure();
                using (var scope = container.BeginLifetimeScope())
                {
                    var app = scope.Resolve<ISettingsBackend>();
                    var success = await app.UpdateUser(new User
                    {
                        Id = user.Id,
                        UName = txtUname.Text,
                        Email = txtEmail.Text,
                        FName = txtFName.Text,
                        LName = txtLName.Text,
                        Pwrd = txtPwrd.Text
                    }, DisplayError);
                    if (success)
                        await DisplayAlert("Updated!", "Your details have been updated!", "Ok");
                }
            }
        }
    }
}