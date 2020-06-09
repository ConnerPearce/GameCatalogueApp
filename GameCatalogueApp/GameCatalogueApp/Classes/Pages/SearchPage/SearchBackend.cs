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

        public async Task<IGameRootObject> GetGames(string search, HomePage.ErrorHandling errorMessage)
        {

            bool connection = _checkConnection.hasConnection(errorMessage);
            if (connection)
            {
                if (!string.IsNullOrEmpty(search))
                {
                    var games = await _gameProxy.GetGameBySearch(search, errorMessage);
                    if (games == null)
                        return null;
                    else
                        return games;
                }
                else
                {
                    var games = await _gameProxy.GetAllGameInfo(errorMessage);
                    if (games == null)
                        return null;
                    else
                        return games;
                }
            }
            else
                return null;
        }

        public async Task<List<Game>> GetCustomGames(string search, HomePage.ErrorHandling errorMessage)
        {
            bool connection = _checkConnection.hasConnection(errorMessage);
            if (connection)
            {
                if (!string.IsNullOrEmpty(search))
                {
                    var games = await _customGameProxy.GetGamesBySearch(errorMessage, search);
                    if (games == null)
                        return null;
                    else
                        return games;
                }
                else
                {
                    var games = await _customGameProxy.GetAllGames(errorMessage);
                    if (games == null)
                        return null;
                    else
                        return games;
                }
            }
            else
                return null;
        }
    }
}
