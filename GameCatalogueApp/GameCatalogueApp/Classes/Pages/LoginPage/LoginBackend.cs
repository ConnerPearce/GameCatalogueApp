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
    // This is the login backend
    // This class manages logging in and saving the user data locally to remember credintials

    public class LoginBackend : ILoginBackend
    {
        private readonly ICheckConnection _checkConnection;
        private readonly IUserProxy _userProxy;

        public LoginBackend(ICheckConnection checkConnection, IUserProxy userProxy)
        {
            _checkConnection = checkConnection;
            _userProxy = userProxy;
        }

        // Gets the user info (Logs them in)
        public async Task<IUser> GetUser(string uName, string password, HomePage.ErrorHandling errorMessage)
        {
            // Checks connection
            bool connection = _checkConnection.hasConnection(errorMessage);
            if (connection)
            {
                // Grabs the user info by their username and password
                IUser user = await _userProxy.GetUser(errorMessage, uName, password);
                if (user != null) // Checks that its not null
                {
                    // If they want their details to be remembered it saves the data locally
                    if (await Storage.ReadTextFileAsync(App.detailsLocation, errorMessage) == "true")
                    {
                        await Storage.WriteTextFileAsync(App.uNameLocation, user.UName, errorMessage);
                        await Storage.WriteTextFileAsync(App.pwrdLocation, user.Pwrd, errorMessage);
                    }

                    return user; // Return the user
                }
                else
                    return null;
            }
            else
                return null;
        }
    }
}
