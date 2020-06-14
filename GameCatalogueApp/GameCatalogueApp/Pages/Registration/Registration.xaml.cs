using Autofac;
using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes.Pages.RegistrationPage;
using GameCatalogueApp.Pages.Home;
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

        private readonly HomePage.ErrorHandling _errorHandling;

        public Registration(HomePage.ErrorHandling errorHandling)
        {
            InitializeComponent();
            _errorHandling = errorHandling;
        }

        protected override void OnDisappearing() 
        { 
            if(container != null)
                container.Dispose();

            // Resets all the fields upon leaving the page
            txtUserName.Text = "";
            txtConfirmedPwrd.Text = "";
            txtPassword.Text = "";
            txtEmail.Text = "";
            txtFName.Text = "";
            txtLName.Text = "";

        }

        private void btnRegister_Clicked(object sender, EventArgs e) => RegisterFunction();

        public async void RegisterFunction()
        {
            if (string.IsNullOrEmpty(txtUserName.Text))
                _errorHandling("Please enter a username");

            else if (string.IsNullOrEmpty(txtFName.Text))
                _errorHandling("Please enter your first name");
            else if (string.IsNullOrEmpty(txtLName.Text))
                _errorHandling("Please enter your last name");
            else if (string.IsNullOrEmpty(txtEmail.Text))
                _errorHandling("Please enter your email");
            else if (string.IsNullOrEmpty(txtPassword.Text))
                _errorHandling("Please enter a password");
            else if (string.IsNullOrEmpty(txtConfirmedPwrd.Text))
                _errorHandling("Please confirm your password");
            else if (txtConfirmedPwrd.Text != txtPassword.Text)
                _errorHandling("Please make sure your passwords match");
            else
            {
                await DisplayAlert("Progress", "Creating account now", "Ok");
                container = DependancyInjection.Configure();
                using (var scope = container.BeginLifetimeScope())
                {
                    var app = scope.Resolve<IRegistrationBackend>();
                    var success = await app.RegisterUser(_errorHandling, new User
                    {
                        UName = txtUserName.Text,
                        Email = txtEmail.Text,
                        FName = txtFName.Text,
                        LName = txtLName.Text,
                        Pwrd = txtPassword.Text
                    });
                    if (success)
                    {
                        await DisplayAlert("Account Created", "Your account has been created\nYou can now sign in", "Ok");
                        await Navigation.PushAsync(new MainPage());
                    }

                }
            }

        }
    }
}