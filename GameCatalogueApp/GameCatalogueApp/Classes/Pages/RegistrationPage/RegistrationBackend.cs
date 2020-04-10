using GameCatalogueApp.Classes.Custom_API.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes.Pages.RegistrationPage
{
    public class RegistrationBackend
    {
        public delegate void ErrorMessage(string message);

        public async Task RegisterUser(ErrorMessage error, User info, string pWord)
        {

        }
    }
}
