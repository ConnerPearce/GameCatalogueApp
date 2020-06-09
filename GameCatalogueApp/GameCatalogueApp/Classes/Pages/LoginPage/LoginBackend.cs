using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes._Custom_API.Proxys;
using GameCatalogueApp.Classes.ConnectionManager;
using GameCatalogueApp.Classes.StorageManager;
using GameCatalogueApp.Pages.Home;
using GameCatalogueApp.Pages.Login;
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

        public LoginBackend(ICheckConnection checkConnection, IUserProxy userProxy)
        {
            _checkConnection = checkConnection;
            _userProxy = userProxy;
        }

        public async Task<IUser> GetUser(string uName, string password, HomePage.ErrorHandling errorMessage)
        {
            bool connection = _checkConnection.hasConnection(errorMessage);
            if (connection)
            {
                IUser user = await _userProxy.GetUser(errorMessage, uName, password);
                if (user != null)
                {
                    if (await Storage.ReadTextFileAsync("rememberDetails.txt", errorMessage) == "true")
                    {
                        await Storage.WriteTextFileAsync("username.txt", user.UName, errorMessage);
                        await Storage.WriteTextFileAsync("password.txt", user.Pwrd, errorMessage);
                    }

                    return user;
                }
                else
                    return null;
            }
            else
                return null;
        }
    }
}
