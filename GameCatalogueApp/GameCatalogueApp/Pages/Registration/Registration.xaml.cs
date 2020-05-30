using Autofac;
using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes.Pages.RegistrationPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameCatalogueApp.Pages.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registration : ContentPage
    {
        private IContainer container;

        public Registration()
        {
            InitializeComponent();
        }

        private void btnRegister_Clicked(object sender, EventArgs e) => RegisterFunction();


        private async void DisplayError(string message) => await DisplayAlert("Something went wrong", $"Error info: {message}", "Ok");


        public async void RegisterFunction()
        {
            if (string.IsNullOrEmpty(txtUserName.Text))
                DisplayError("Please enter a username");

            else if (string.IsNullOrEmpty(txtFName.Text))
                DisplayError("Please enter your first name");
            else if (string.IsNullOrEmpty(txtLName.Text))
                DisplayError("Please enter your last name");
            else if (string.IsNullOrEmpty(txtEmail.Text))
                DisplayError("Please enter your email");
            else if (string.IsNullOrEmpty(txtPassword.Text))
                DisplayError("Please enter a password");
            else if (string.IsNullOrEmpty(txtConfirmedPwrd.Text))
                DisplayError("Please confirm your password");
            else
            {
                await DisplayAlert("Progress", "Creating account now", "Ok");
                container = DependancyInjection.Configure();
                using (var scope = container.BeginLifetimeScope())
                {
                    var app = scope.Resolve<IRegistrationBackend>();
                    var success = await app.RegisterUser(DisplayError, new User { 
                        UName = txtUserName.Text, 
                        Email = txtEmail.Text, 
                        FName = txtFName.Text,
                        LName = txtLName.Text,
                        Pwrd = txtPassword.Text
                    });
                    if (success)
                        await DisplayAlert("Account Created", "Your account has been created\nYou can now sign in", "Ok");
                }
            }

        }
    }
}