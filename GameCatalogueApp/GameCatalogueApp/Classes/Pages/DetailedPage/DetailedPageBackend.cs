using GameCatalogueApp.API;
using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes._Custom_API.Proxys;
using GameCatalogueApp.Classes.API.Data;
using GameCatalogueApp.Classes.ConnectionManager;
using GameCatalogueApp.Pages.DetailedItem;
using GameCatalogueApp.Pages.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes.Pages.DetailedPage
{
    public class DetailedPageBackend : IDetailedPageBackend
    {
        // Dependancy Injection Variables
        private readonly ICheckConnection _checkConnection;
        private readonly IGameProxy _gameProxy;
        private readonly ICustomGameProxy _customGameProxy;
        private readonly IWishlistPlayedProxy _wishlistPlayedProxy;


        // Dependancy injection resolves all the variables the constructor takes in 
        public DetailedPageBackend(ICheckConnection checkConnection, IGameProxy gameProxy, ICustomGameProxy customGameProxy, IWishlistPlayedProxy wishlistPlayedProxy)
        {
            _checkConnection = checkConnection;
            _gameProxy = gameProxy;
            _customGameProxy = customGameProxy;
            _wishlistPlayedProxy = wishlistPlayedProxy;
        }

        // Retrieves a single game from the RAWG Api //
        public async Task<ISingleGameRootObject> GetGame(string slug, HomePage.ErrorHandling errorMessage) =>
            // First checks if there is a connection
            // If there is a connection it will Get the Game info from the API
            // The ?? checks if the item is null, if its null then it will return null, if its not null then it will return the ISingleGameRoot Object
            // Finally if the there is no connection then this method returns null
             _checkConnection.hasConnection(errorMessage) ? await _gameProxy.GetSinlgeGameInfo(slug, errorMessage) ?? null : null;           

        // Retrieves a single game from custom API //
        public async Task<IGame> GetCustomGame(string id, HomePage.ErrorHandling errorMessage) =>
             _checkConnection.hasConnection(errorMessage) ? await _customGameProxy.GetGameByID(errorMessage, id) ?? null : null;


        // A generic method that handles adding to wishlist or completed games and returns true or false based on success //
        public async Task<bool> AddToWishlistPlayed<T>(HomePage.ErrorHandling errorMessage, T item, string itemChoice) =>
             // Checks if there is a connection
             // If there is then it will try to post a an item and return true or false based wether the item was able to be posted or not
             // If there is no connection then it returns false
             _checkConnection.hasConnection(errorMessage) ? await _wishlistPlayedProxy.PostItem(errorMessage, item, itemChoice) : false;


        // A generic Delete function that returns true or false based on success //
        public async Task<bool> DeleteFromWishlistPlayed(HomePage.ErrorHandling errorMessage, string itemChoice, string id) =>
             _checkConnection.hasConnection(errorMessage) ? await _wishlistPlayedProxy.DeleteItem(errorMessage, itemChoice, id) : false;


        // This method checks if the item they are trying to add is already in the wishlist table
        // Used to switch buttons from add to remove
        public async Task<bool> IsOnWishlist(HomePage.ErrorHandling errorMessage, Game item, string id)
        {
            bool connection = _checkConnection.hasConnection(errorMessage);
            if (connection)
            {
                var items = await _wishlistPlayedProxy.IsOnT<Wishlist>("Wishlist", id);
                if (items != null)
                {
                    // Filters through the item to find if there are any items where the game id is already in there
                    var temp = items.Where(e => e.GameID == item.id).FirstOrDefault();
                    if (temp != null)
                    {
                        // Sets the ID so the user can delete the item later on
                        GameCatalogueApp.Pages.DetailedItem.DetailedPage.wishlistID = temp.Id;
                        return true;
                    }
                }
                return false;
            }
            else
                return false;
        }

        // This method checks if the item they are trying to add is already in the Played table
        // Used to switch buttons from add to remove
        public async Task<bool> IsOnPlayed(HomePage.ErrorHandling errorMessage, Game item, string id)
        {
            bool connection = _checkConnection.hasConnection(errorMessage);
            if (connection)
            {
                var items = await _wishlistPlayedProxy.IsOnT<Played>("Played", id); // Grabs all items associated with a user id
                if (items != null)
                {
                    // Filters through the item to find if there are any items where the game id is already in there
                    var temp = items.Where(e => e.GameID == item.id).FirstOrDefault();
                    if (temp != null)
                    {
                        // Sets the ID so the user can delete the item later on
                        GameCatalogueApp.Pages.DetailedItem.DetailedPage.playedID = temp.Id;
                        return true;
                    }
                }
                return false;
            }
            else
                return false;
        }
    }
}
