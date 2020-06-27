using GameCatalogueApp.API;
using GameCatalogueApp.API.Data;
using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes._Custom_API.Proxys;
using GameCatalogueApp.Classes.ConnectionManager;
using GameCatalogueApp.Pages.Home;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes
{
    // Manages searching for games in my program

    public class SearchBackend : ISearchBackend
    {
        // Dependancy Injection Variables
        private readonly ICheckConnection _checkConnection;
        private readonly IGameProxy _gameProxy;
        private readonly ICustomGameProxy _customGameProxy;

        public SearchBackend(ICheckConnection checkConnection, IGameProxy gameProxy, ICustomGameProxy customGameProxy)
        {
            _checkConnection = checkConnection;
            _gameProxy = gameProxy;
            _customGameProxy = customGameProxy;
        }

        // Gets games from Rawg Database
        public async Task<IGameRootObject> GetGames(string search, HomePage.ErrorHandling errorMessage)
        {
            // Checks connection
            bool connection = _checkConnection.hasConnection(errorMessage);
            if (connection)
            {
                // This is short hand for the below code, this makes it more difficult for me and reduces the amount of If statements greatly
                // Could be shortened even more but it would look much more confusing after removing yet another if statement
                return !string.IsNullOrEmpty(search) ? await _gameProxy.GetGameBySearch(search, errorMessage) ?? null : await _gameProxy.GetAllGameInfo(errorMessage) ?? null;

                // Original Code

                // If it has something to search for
                //if (!string.IsNullOrEmpty(search))
                //{
                //    // Gets games by search
                //    var games = await _gameProxy.GetGameBySearch(search, errorMessage);
                //    return games ?? null;

                //}
                //else // If their is no input it should grab all games
                //{
                //    // Gets all games
                //    var games = await _gameProxy.GetAllGameInfo(errorMessage);
                //    return games ?? null;
                //}
            }
            else
                return null;
        }

        // Gets games from my Custom API
        // Works the same as above but it gets it from my custom API instead of the Rawg API
        public async Task<List<Game>> GetCustomGames(string search, HomePage.ErrorHandling errorMessage)
        {
            bool connection = _checkConnection.hasConnection(errorMessage);
            if (connection)
            {
                // Shorthand code
                return !string.IsNullOrEmpty(search) ? await _customGameProxy.GetGamesBySearch(errorMessage, search) ?? null : await _customGameProxy.GetAllGames(errorMessage) ?? null;
            }
            else
                return null;
        }
    }
}
