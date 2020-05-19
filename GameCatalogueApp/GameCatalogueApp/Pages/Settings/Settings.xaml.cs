using GameCatalogueApp.Classes._Custom_API.Data;
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
        private IUser user = new User();
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
                txtUname.Text = user.UName;
                txtFName.Text = user.FName;
                txtLName.Text = user.LName;
                txtEmail.Text = user.Email;
                txtPwrd.Text = user.Pwrd;
            }
            else
                tblAccountSettings.IsVisible = false;

        }
    }
}