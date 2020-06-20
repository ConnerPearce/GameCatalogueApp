using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes._Custom_API.Proxys;
using GameCatalogueApp.Classes.ConnectionManager;
using GameCatalogueApp.Pages.Home;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes.Pages.WishlistPlayedPage
{
    // This page manages the main functionality of my wishlist

    public class WishlistPlayedBackend : IWishlistPlayedBackend
    {
        private readonly ICheckConnection _checkConnection;
        private readonly IWishlistPlayedProxy _wishlistPlayedProxy;
        private readonly ICustomGameProxy _customGameProxy;


        public WishlistPlayedBackend(ICheckConnection checkConnection, IWishlistPlayedProxy wishlistPlayedProxy, ICustomGameProxy customGameProxy)
        {
            _checkConnection = checkConnection;
            _wishlistPlayedProxy = wishlistPlayedProxy;
            _customGameProxy = customGameProxy;
        }

        // Get the wishlist
        public async Task<List<Game>> GetWishlist(HomePage.ErrorHandling errorMessage, string id)
        {
            // Checks the internet connection
            bool connection = _checkConnection.hasConnection(errorMessage);
            if (connection)
            {
                // Checks the id isnt null
                if (id != null)
                {
                    // Gets the wishlist 
                    var items = await _wishlistPlayedProxy.GetItems<Wishlist>(errorMessage, "Wishlist", id);

                    // If it was able to get items back
                    if (items != null)
                    {
                        List<Game> games = new List<Game>();

                        // Gets all the games asociated with the wishlist gameID
                        // Since MongoDB cant do inner joins this was the closest thing i could do
                        foreach (var item in items)
                        {
                            // Each item in the wishlist a search will be made that grabs the game matching its ID
                            games.Add((Game)await _customGameProxy.GetGameByID(errorMessage, item.GameID));
                        }
                        return games;
                    }
                    else
                        return null;
                }
                else
                {
                    // Using my delegate it will return the error message that the user must be logged in
                    errorMessage("Must be logged in");
                    return null;
                }
            }
            else
                return null;
        }

        // Gets the items on played table
        // Essentially mirrors the GetWishlist but instead gets the played items
        public async Task<List<Game>> GetPlayed(HomePage.ErrorHandling errorMessage, string id)
        {
            bool connection = _checkConnection.hasConnection(errorMessage);
            if (connection)
            {
                if (id != null)
                {
                    var items = await _wishlistPlayedProxy.GetItems<Played>(errorMessage, "Played", id);
                    if (items != null)
                    {
                        List<Game> games = new List<Game>();
                        foreach (var item in items)
                        {
                            games.Add((Game)await _customGameProxy.GetGameByID(errorMessage, item.GameID));
                        }
                        return games;
                    }
                    else
                        return null;
                }
                else
                {
                    errorMessage("Must be logged in");
                    return null;
                }
            }
            else
                return null;

        }
    }
}
