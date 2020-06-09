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
    public class RegistrationBackend : IRegistrationBackend
    {
        private readonly ICheckConnection _checkConnection;
        private readonly IUserProxy _userProxy;

        public RegistrationBackend(ICheckConnection checkConnection, IUserProxy userProxy)
        {
            _checkConnection = checkConnection;
            _userProxy = userProxy;
        }

        public async Task<bool> RegisterUser(HomePage.ErrorHandling errorMessage, User user)
        {
            bool connection = _checkConnection.hasConnection(errorMessage);
            if (connection)
                return await _userProxy.PostUser(errorMessage, user);
            else
                return false;
        }
    }
}
