using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes._Custom_API.Proxys;
using GameCatalogueApp.Classes.ConnectionManager;
using GameCatalogueApp.Pages.Home;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes.Pages.SettingsPage
{
    // This manages my settings page and serves as the backend

    public class SettingsBackend : ISettingsBackend
    {
        private readonly ICheckConnection _checkConnection;
        private readonly IUserProxy _userProxy;

        public SettingsBackend(ICheckConnection checkConnection, IUserProxy userProxy)
        {
            _checkConnection = checkConnection;
            _userProxy = userProxy;
        }

        // This method updates the user details
        public async Task<bool> UpdateUser(User user, HomePage.ErrorHandling errorMessage)
        {
            // This is a short hand form of the code below, I learnt to do it this way to shorten down code and challenge myself to minimize my code
            return _checkConnection.hasConnection(errorMessage) ? await _userProxy.PutUser(errorMessage, user) : false;

            // Original Code

            //bool connection = _checkConnection.hasConnection(errorMessage);
            //if (connection)
            //    return await _userProxy.PutUser(errorMessage, user); 
            //else
            //    return false; 
        }
    }
}
