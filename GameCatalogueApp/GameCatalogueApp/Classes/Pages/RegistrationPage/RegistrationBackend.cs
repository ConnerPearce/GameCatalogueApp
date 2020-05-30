using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes._Custom_API.Proxys;
using GameCatalogueApp.Classes.ConnectionManager;
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

        private string errorInfo;

        public delegate void ErrorMessage(string message);

        public RegistrationBackend(ICheckConnection checkConnection, IUserProxy userProxy)
        {
            _checkConnection = checkConnection;
            _userProxy = userProxy;
        }

        public async Task<bool> RegisterUser(ErrorMessage errorMessage, User user)
        {
            bool connection = _checkConnection.hasConnection((error) => errorInfo = error);
            if (connection)
                return await _userProxy.PostUser((error) => errorInfo = error, user);
            else
            {
                errorMessage(errorInfo);
                return false;
            }

        }
    }
}
