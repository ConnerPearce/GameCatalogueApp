using Autofac;
using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes.Pages.LoginPage;
using GameCatalogueApp.Classes.StorageManager;
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

        private readonly HomePage.ErrorHandling _errorHandling;

        public Login(HomePage.ErrorHandling errorHandling)
        {
            InitializeComponent();
            _errorHandling = errorHandling;
        }

        protected override async void OnAppearing()
        {
            chkRemember.IsChecked = (await Storage.ReadTextFileAsync(App.detailsLocation, _errorHandling) == "true") ? true : false;
        }


        // When the navigation is moving from this page, Clear up resources used by dependancy injection
        protected override void OnDisappearing()
        {
            if (container != null)
                container.Dispose();
        }

        private async void btnRegistration_Clicked(object sender, EventArgs e) => await Navigation.PushAsync(new Registration.Registration(_errorHandling));

        private void Login_Clicked(object sender, EventArgs e) => LoginFunction();


        public async void LoginFunction()
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
                _errorHandling("Please enter a username");
            else if (string.IsNullOrEmpty(txtPassword.Text))
                _errorHandling("Please enter a password");
            else
            {
                container = DependancyInjection.Configure();
                using (var scope = container.BeginLifetimeScope())
                {
                    var option = chkRemember.IsChecked ? "true" : "false";
                    await Storage.WriteTextFileAsync(App.detailsLocation, option, _errorHandling);

                    var app = scope.Resolve<ILoginBackend>();
                    var user = await app.GetUser(txtUsername.Text, txtPassword.Text, _errorHandling);

                    // This is a shorthand version of an if else statement i had in my program, This way is less code and also works faster
                    // Technically it is the same as the if else statement commented out below but is much faster
                    var temp = user != null ? (App.isLoggedIn = true, App.user = user) : (App.isLoggedIn = false, App.user = new User());
                    if (temp.Item1)
                        await Navigation.PushAsync(new MainPage());

                    // Below is an expanded version of the code above

                    //if (user != null)
                    //{
                    //    App.isLoggedIn = true;
                    //    App.user = user;

                    //    await Navigation.PushAsync(new MainPage());
                    //}
                    //else
                    //{
                    //    App.isLoggedIn = false;
                    //    App.user = new User();
                    //}
                }
            }           
        }
    }
}