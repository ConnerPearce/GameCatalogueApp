using Autofac;
using GameCatalogueApp.API.Data;
using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes.API.Data;
using GameCatalogueApp.Classes.Pages.DetailedPage;
using GameCatalogueApp.Pages.Home;
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
        private IGame game;
        private readonly string _id;
        private bool isRunning;
        public static string wishlistID;
        public static string playedID;

        // Creating local instances of the delegates to use in this page
        private HomePage.LoginFunction _loginFunction;
        private HomePage.UserFunction _userFunction;
        private HomePage.ErrorHandling _errorHandling;

        // id is the unique id of the record from the database, in the Rawg API its in the form of a slug, in the custom API its in the form of an ObjectID
        // rawgOrCustom is a bool. Depending on where the item is being stored the app will do something different to retrieve it differently
        // It will be set to true if its from the Rawg db, and it will be true if its from my custom db
        public DetailedPage(string id, HomePage.LoginFunction loginFunction, HomePage.UserFunction userFunction, HomePage.ErrorHandling errorHandling)
        {
            _id = id;
            _loginFunction = loginFunction;
            _userFunction = userFunction;
            _errorHandling = errorHandling;

            InitializeComponent();
        }

        //When this page is navigated to this method is fired
        protected override void OnAppearing()
        {
            // Sets my event handlers for the .clicked function
            if (App.isLoggedIn)
            {
                btnLoginUser.Text = App.user.UName;
                btnLoginUser.Clicked += new EventHandler(_userFunction);
            }
            else
            {
                btnLoginUser.Text = "Login";
                btnLoginUser.Clicked += new EventHandler(_loginFunction);

                btnCompleted.IsVisible = false;
                btnRemoveComp.IsVisible = false;
                btnWishlist.IsVisible = false;
                btnRemoveWish.IsVisible = false;
            }
            if(!isRunning)
            {
                isRunning = true;
                InsertInfo(_id);
                isRunning = false;
            }
        }

        // When the page is being navigated away from this method is fired
        // Clears up resources used by dependancy injection
        protected override void OnDisappearing() 
        {
            if (App.isLoggedIn)
                btnLoginUser.Clicked -= new EventHandler(_userFunction);
            else
                btnLoginUser.Clicked -= new EventHandler(_loginFunction);
            if(container != null)
                container.Dispose();
        } 


        private async void InsertInfo(string id)
        {
            container = DependancyInjection.Configure();
            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IDetailedPageBackend>();

                // If the app is using  the custom API then it makes different calls
                if (App.useCustomAPI)
                {
                    game = await app.GetCustomGame(id, _errorHandling);
                    if (game != null) // No need for an else, if its null DisplayError will handle the error on its own
                    {
                        if (App.isLoggedIn)
                        {
                            var isWishlist = await app.IsOnWishlist(_errorHandling, (Game)game, App.user.Id);
                            // This will change the visibility of buttons, so if the game is on the wishlist it will change the button being displayed to Remove From Wishlist as opposed to Add To Wishlist
                            btnRemoveWish.IsVisible = isWishlist;
                            // If the isWishlist is true then set the button visibility to false ? means (Condition ? If its true do this : if its false do this)
                            btnWishlist.IsVisible = isWishlist ? false : true;

                            var isPlayed = await app.IsOnPlayed(_errorHandling, (Game)game, App.user.Id);
                            btnRemoveComp.IsVisible = isPlayed;
                            btnCompleted.IsVisible = isPlayed ? false : true;
                        }
                        // Below just fills the xaml with text, Binding wouldnt work here as i need to filter through lists
                        lblGameName.Text = game.name;
                        lblGenre.Text = game.Genre;
                        lblDeveloper.Text = game.Developer;
                        // This filters through each platform in the list then concatenates it to the lblPlatforms.Text to be displayed
                        foreach (var item in game.Platform)
                        {
                            lblPlatforms.Text += $"{item}, ";
                        }
                        lblRating.Text = game.Rating.ToString();
                        imgGamePhoto = new Image()
                        {
                            Aspect = Aspect.AspectFit,
                            Source = ImageSource.FromUri(new Uri(game.background_image)) // This gets the image from the webpage and displays it
                        };
                        lblSummary.Text = game.Summary;
                    }
                }
                else // Uses Rawg API
                {
                    var game = await app.GetGame(id, _errorHandling);
                    if (game != null)
                    {
                        lblGameName.Text = game.name;
                        lblGenre.Text = game.genres.First().name;
                        lblDeveloper.Text = game.publishers.First().name;
                        foreach (var item in game.platforms)
                        {
                            lblPlatforms.Text += $"{item.platform.name}, ";
                        }
                        lblRating.Text = game.rating.ToString();
                        imgGamePhoto = new Image()
                        {
                            Aspect = Aspect.AspectFit,
                            Source = ImageSource.FromUri(new Uri(game.background_image))
                        };
                        lblSummary.Text = game.description;
                    }
                }
            }
        }

        // This is a shared event handler
        private async void AddToList(object sender, EventArgs e)
        {
            try
            {
                container = DependancyInjection.Configure();
                using (var scope = container.BeginLifetimeScope())
                {
                    var app = scope.Resolve<IDetailedPageBackend>();
                    var btn = (Button)sender;
                    string choice = string.Empty;
                    bool success = false;

                    // By checking the senders ID i can see which button sent the click event
                    if (btn.Id == btnCompleted.Id)
                    {
                        choice = "Played";
                        success = await app.AddToWishlistPlayed(_errorHandling, new Classes._Custom_API.Data.Played { GameID = game.id, UId = App.user.Id }, choice);

                        if (success)
                        {
                            btnRemoveComp.IsVisible = true;
                            btnCompleted.IsVisible = false;
                        }
                    }
                    else if(btn.Id == btnWishlist.Id)
                    {
                        choice = "Wishlist";

                        success = await app.AddToWishlistPlayed(_errorHandling, new Classes._Custom_API.Data.Wishlist { GameID = game.id, UId = App.user.Id }, choice);
                        if (success)
                        {
                            btnRemoveComp.IsVisible = true;
                            btnCompleted.IsVisible = false;
                        }
                    }
                    if (success)
                        await DisplayAlert("Added Item", $"Added {game.name} to {choice}", "Ok");
                }
            }
            catch (Exception ex)
            {
                _errorHandling(ex.Message);
            }
        }

        private async void RemoveFromList(object sender, EventArgs e)
        {
            try
            {
                container = DependancyInjection.Configure();
                using (var scope = container.BeginLifetimeScope())
                {
                    var app = scope.Resolve<IDetailedPageBackend>();
                    var btn = (Button)sender;
                    string choice = string.Empty;
                    bool success = false;
                    if (btn.Id == btnRemoveComp.Id)
                    {
                        choice = "Played";

                        success = await app.DeleteFromWishlistPlayed(_errorHandling, "Played", playedID);
                        if (success)
                        {
                            btnCompleted.IsVisible = true;
                            btnRemoveComp.IsVisible = false;
                        }
                    }
                    else if (btn.Id == btnRemoveWish.Id)
                    {
                        choice = "Wishlist";

                        success = await app.DeleteFromWishlistPlayed(_errorHandling, "Wishlist", wishlistID);
                        if (success)
                        {
                            btnWishlist.IsVisible = true;
                            btnRemoveWish.IsVisible = false;
                        }

                    }
                    if (success)
                        await DisplayAlert("Item Removed", $"Removed {game.name} from {choice}", "Ok");
                }
            }
            catch (Exception ex)
            {
                _errorHandling(ex.Message);
            }
        }
    }
}