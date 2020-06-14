using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Pages.Home;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes._Custom_API.Proxys
{
    public class UserProxy : IUserProxy
    {
        // USED FOR DEPENDANCY INJECTION
        private readonly string _baseAddress;

        public UserProxy(string baseAddress)
        {
            _baseAddress = baseAddress;
        }

        // Checks the status codes for the HttpResponse and returns true or false and a error message if needed
        private async Task<bool> CheckStatusCodes(HttpResponseMessage response, HomePage.ErrorHandling errorMessage)
        {
            // This handles all status errors
            if (response.IsSuccessStatusCode)
                return true;
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                errorMessage("No User was found with those details, Please try again");
            }
            else // Any unprepared status code
            {
                errorMessage($"\nStatus Code: {response.StatusCode} \nError Message: {response.ReasonPhrase}");

                // Below is how i tested getting advanced error info to give to Sam
                errorMessage($"Advanced Info: {await response.Content.ReadAsStringAsync()}");
            }
            return false;
        }

        // Used to retrieve a single user, for logging in, Returns an IUser
        public async Task<IUser> GetUser(HomePage.ErrorHandling errorMessage, string uName, string password)
        {
            try
            {
                var http = new HttpClient
                {
                    BaseAddress = new Uri(_baseAddress)
                };

                var url = $"user/user={uName}&pwrd={password}";
                HttpResponseMessage response = http.GetAsync(url).Result;
                if (await CheckStatusCodes(response, errorMessage))
                {
                    var user = response.Content.ReadAsAsync<User>();
                    return await user;
                }
                else
                    return null;
            }
            catch (Exception ex) 
            {
                // Returns the error to the user so they can report it
                // More of so my app doesnt crash if anything unexpected happens while connecting to the API
                // (I havent found any errors in testing but better to be prepared then for it to crash)
                errorMessage(ex.Message);
                return null;
            }          
        }

        // Used to create a new user, used in registration, Returns a bool depending on success
        public async Task<bool> PostUser(HomePage.ErrorHandling errorMessage, User user)
        {
            try
            {
                var http = new HttpClient();

                var response = await http.PostAsJsonAsync($"{_baseAddress}User", user);

                return await CheckStatusCodes(response, errorMessage) ? true : false;
            }            
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return false;
            }
        }

        // Updates user info, used in settings page, Returns a bool depending on success
        public async Task<bool> PutUser(HomePage.ErrorHandling errorMessage, User user)
        {
            try
            {
                var http = new HttpClient();

                var response = await http.PutAsJsonAsync($"{_baseAddress}User", user);

                return await CheckStatusCodes(response, errorMessage) ? true : false;
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return false;
            }
        }
    }
}
