using Autofac;
using GameCatalogueApp.API.Data;
using GameCatalogueApp.Classes.Pages.DetailedPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GameCatalogueApp.Pages.DetailedItem
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailedPage : ContentPage
    {
        private IContainer container;
        public DetailedPage(string slug)
        {
            InitializeComponent();
            InsertInfo(slug);
        }
        // The Method for Alert Displays that will be passed using delegates
        // HANDLES ALL ERROR MESSAGES //
        private async void DisplayError(string error) => await DisplayAlert("Something went wrong", $"Error info: {error}", "Ok");

        // LOGIN BUTTON
        private async void btnLogin_Clicked(object sender, EventArgs e) => await Navigation.PushAsync(new Login.Login());

        private async void InsertInfo(string id)
        {
            container = DependancyInjection.Configure();
            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IDetailedPageBackend>();
                var games = await app.GetGame(id, DisplayError);
                if (games != null)
                {
                    lblGameName.Text = games.name;
                    lblGenre.Text = games.genres.First().name;
                    lblDeveloper.Text = games.publishers.First().name;
                    foreach (var item in games.platforms)
                    {
                        lblPlatforms.Text += $"{item.platform.name}, ";
                    }
                    lblRating.Text = games.rating.ToString();
                    imgGamePhoto = new Image()
                    {
                        Aspect = Aspect.AspectFit,
                        Source = ImageSource.FromUri(new Uri(games.background_image))
                    };
                    lblSummary.Text = games.description;
                }
            }
        }

    }
}