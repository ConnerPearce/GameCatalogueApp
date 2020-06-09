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
    public class WishlistPlayedBackend : IWishlistPlayedBackend
    {
        ICheckConnection _checkConnection;
        IWishlistPlayedProxy _wishlistPlayedProxy;
        ICustomGameProxy _customGameProxy;


        public WishlistPlayedBackend(ICheckConnection checkConnection, IWishlistPlayedProxy wishlistPlayedProxy, ICustomGameProxy customGameProxy)
        {
            _checkConnection = checkConnection;
            _wishlistPlayedProxy = wishlistPlayedProxy;
            _customGameProxy = customGameProxy;
        }

        public async Task<List<Game>> GetWishlist(HomePage.ErrorHandling errorMessage, string id)
        {
            bool connection = _checkConnection.hasConnection(errorMessage);
            if (connection)
            {
                if (id != null)
                {
                    var items = await _wishlistPlayedProxy.GetItems<Wishlist>(errorMessage, "Wishlist", id);
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
