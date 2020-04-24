using GameCatalogueApp.API;
using GameCatalogueApp.Classes.API.Data;
using GameCatalogueApp.Classes.ConnectionManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes.Pages.DetailedPage
{
    public class DetailedPageBackend : IDetailedPageBackend
    {
        // Dependancy Injection Variables
        private readonly ICheckConnection _checkConnection;
        private readonly IGameProxy _gameProxy;

        // Class Specific Variables
        private string errorInfo;

        // Delegate for errors
        public delegate void ErrorMessage(string message);

        public DetailedPageBackend(ICheckConnection checkConnection, IGameProxy gameProxy)
        {
            _checkConnection = checkConnection;
            _gameProxy = gameProxy;
        }

        public async Task<ISingleGameRootObject> GetGame(string slug, ErrorMessage errorMessage)
        {

            bool connection = _checkConnection.hasConnection((error) => errorInfo = error);
            if (connection)
            {
                ISingleGameRootObject games = await _gameProxy.GetSinlgeGameInfo(slug, (error) => errorInfo = error);
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
                errorMessage(errorInfo);
                return null;
            }
        }
    }
}
