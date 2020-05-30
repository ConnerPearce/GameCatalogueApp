using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes._Custom_API.Proxys;
using GameCatalogueApp.Classes.ConnectionManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes.Pages.SettingsPage
{
    public class SettingsBackend : ISettingsBackend
    {
        private readonly IUserProxy _userProxy;
        private readonly ICheckConnection _checkConnection;

        private string errorInfo;
        public delegate void ErrorMessage(string error);

        public SettingsBackend(ICheckConnection checkConnection, IUserProxy userProxy)
        {
            _checkConnection = checkConnection;
            _userProxy = userProxy;
        }

        public async Task<bool> UpdateUser(User user, ErrorMessage errorMessage)
        {
            bool connection = _checkConnection.hasConnection((error) => errorInfo = error);
            if (connection)
            {
                var updated = await _userProxy.PutUser((error) => errorInfo = error, user);
                if (updated)
                    return true;
                else
                {
                    errorMessage(errorInfo);
                    return false;
                }
            }
            else
            {
                errorMessage(errorInfo);
                return true;
            }
        }

    }
}
