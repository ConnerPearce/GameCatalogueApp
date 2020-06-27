using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes._Custom_API.Proxys;
using GameCatalogueApp.Classes.ConnectionManager;
using GameCatalogueApp.Pages.Home;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes.Pages.RegistrationPage
{
    // This is the backend for my registration page
    // It manages the registration section

    public class RegistrationBackend : IRegistrationBackend
    {
        // Dependancy injection variables
        private readonly ICheckConnection _checkConnection;
        private readonly IUserProxy _userProxy;

        public RegistrationBackend(ICheckConnection checkConnection, IUserProxy userProxy)
        {
            _checkConnection = checkConnection;
            _userProxy = userProxy;
        }

        // Registers the user
        public async Task<bool> RegisterUser(HomePage.ErrorHandling errorMessage, User user) =>
            // Checks if there is a connection
            // If there is then it will try to post a user and return true or false based wether the user was able to be posted or not
            // If there is no connection then it returns false
            _checkConnection.hasConnection(errorMessage) ? await _userProxy.PostUser(errorMessage, user) : false;

    }
}
