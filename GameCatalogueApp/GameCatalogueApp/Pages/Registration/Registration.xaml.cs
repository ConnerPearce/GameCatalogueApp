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
    // This page is my registration page
    // It manages registering a user

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registration : ContentPage
    {
        private IContainer container;

        // My error handling delegate
        private readonly HomePage.ErrorHandling _errorHandling;

        public Registration(HomePage.ErrorHandling errorHandling)
        {
            _errorHandling = errorHandling;
            InitializeComponent();
        }

        // This is called upon leaving the page
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

        // Button click will call my RegisterFunction method
        private void btnRegister_Clicked(object sender, EventArgs e) => RegisterFunction();

        // Registers a user
        public async void RegisterFunction()
        {
            // Validation check, displays an error if somethings wrong with appropriate message
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
                // Informs the user their account is being created
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
                        // If the user is created successfully then it will say so and push you to the main page
                        await DisplayAlert("Account Created", "Your account has been created\nYou can now sign in", "Ok");
                        await Navigation.PushAsync(new MainPage());
                    }

                    // If the user isnt created successfully my Error Handling Delegate will display the error reason
                }
            }

        }
    }
}