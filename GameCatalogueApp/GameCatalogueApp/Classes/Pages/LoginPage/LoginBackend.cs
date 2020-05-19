using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes._Custom_API.Proxys;
using GameCatalogueApp.Classes.ConnectionManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes.Pages.LoginPage
{
    public class LoginBackend : ILoginBackend
    {
        private readonly ICheckConnection _checkConnection;
        private readonly IUserProxy _userProxy;

        private string errorInfo;

        public delegate void ErrorMessage(string message);

        public LoginBackend(ICheckConnection checkConnection, IUserProxy userProxy)
        {
            _checkConnection = checkConnection;
            _userProxy = userProxy;
        }

        public async Task<IUser> GetUser(string uName, string password, ErrorMessage errorMessage)
        {
            bool connection = _checkConnection.hasConnection((error) => errorInfo = error);
            if (connection)
            {
                IUser user = await _userProxy.GetUser((error) => errorInfo = error, uName, password);
                if (user != null)
                {
                    return user;
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
