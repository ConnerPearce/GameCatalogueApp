using GameCatalogueApp.API;
using GameCatalogueApp.API.Data;
using GameCatalogueApp.Classes.ConnectionManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes
{
    public class SearchBackend
    {
        // Dependancy Injection Variables
        private readonly CheckConnection _checkConnection;
        private readonly GameProxy _gameProxy;

        // Class Specific Variables
        private string errorInfo;

        // Delegate for errors
        public delegate void ErrorMessage(string message);
        public SearchBackend(CheckConnection checkConnection, GameProxy gameProxy)
        {
            _checkConnection = checkConnection;
            _gameProxy = gameProxy;
        }

        public async Task<IGameRootObject> GetGames(string search, ErrorMessage errorMessage)
        {
            try
            {
                bool connection = _checkConnection.hasConnection((error) => errorInfo = error);
                if (connection)
                {
                    if (!string.IsNullOrEmpty(search))
                    {
                        IGameRootObject games = await _gameProxy.GetGameBySearch(search, (error) => errorInfo = error);
                        if (games != null)
                        {
                            return games;
                        }
                        else
                        {
                            errorMessage(errorInfo);
                            return null;
                        }
                    }
                    else
                    {
                        IGameRootObject games = await _gameProxy.GetAllGameInfo((error) => errorInfo = error);
                        if (games != null)
                        {
                            return games;
                        }
                        else
                        {
                            errorMessage(errorInfo);
                            return null;
                        }
                    }
                }
                else
                {
                    errorMessage(errorInfo);
                    return null;
                }
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return null;
            }
        }
    }
}
