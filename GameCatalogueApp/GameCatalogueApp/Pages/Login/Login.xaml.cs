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
        public Login()
        {
            InitializeComponent();
        }
        private async void btnRegistration_Clicked(object sender, EventArgs e) => await Navigation.PushAsync(new Registration.Registration());

        private void Login_Clicked(object sender, EventArgs e)
        {

        }

        private void chkRemember_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {

        }
    }
}