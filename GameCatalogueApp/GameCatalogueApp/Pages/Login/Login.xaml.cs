using Autofac;
using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes.Pages.LoginPage;
using GameCatalogueApp.Pages.Home;
using GameCatalogueApp.Pages.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameCatalogueApp.Pages.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        private IContainer container;

        public Login()
        {
            InitializeComponent();
        }

        // The Method for Alert Displays that will be passed using delegates
        // HANDLES ALL ERROR MESSAGES //
        private async void DisplayError(string error) => await DisplayAlert("Something went wrong", $"Error info: {error}", "Ok");

        private async void btnRegistration_Clicked(object sender, EventArgs e) => await Navigation.PushAsync(new Registration.Registration());

        private void Login_Clicked(object sender, EventArgs e) => LoginFunction();


        public async void LoginFunction()
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
                DisplayError("Please enter a username");
            else if (string.IsNullOrEmpty(txtPassword.Text))
                DisplayError("Please enter a password");
            else
            {
                container = DependancyInjection.Configure();
                using (var scope = container.BeginLifetimeScope())
                {
                    var app = scope.Resolve<ILoginBackend>();
                    var user = await app.GetUser(txtUsername.Text, txtPassword.Text, DisplayError);
                    if (user != null)
                    {
                        HomePage.isLoggedIn = true;
                        HomePage.user = user;
                        await DisplayAlert("User Info Retrieved!", $"Hello {user.FName} {user.LName}", "Hello");
                    }
                    else
                    {
                        HomePage.isLoggedIn = false;
                        HomePage.user = new User();
                    }
                }
            }           
        }

        private void chkRemember_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

        }
    }
}