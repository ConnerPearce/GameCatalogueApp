using GameCatalogueApp.Classes._Custom_API.Data;
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

        public delegate void ErrorMessage(string message);

        public UserProxy(string baseAddress)
        {
            _baseAddress = baseAddress;
        }

        // Checks the status codes for the HttpResponse and returns true or false and a error message if needed
        private bool CheckStatusCodes(HttpResponseMessage response, ErrorMessage errorMessage)
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
                errorMessage($"Something went wrong! \nStatus Code: {response.StatusCode} \nError Message{response.ReasonPhrase}");
            }
            return false;
        }

        // Used to retrieve a single user, for logging in
        public async Task<IUser> GetUser(ErrorMessage errorMessage, string uName, string password)
        {
            try
            {
                var http = new HttpClient
                {
                    BaseAddress = new Uri(_baseAddress)
                };

                var url = $"user/user={uName}&pwrd={password}";
                HttpResponseMessage response = http.GetAsync(url).Result;
                if (CheckStatusCodes(response, errorMessage))
                {
                    var user = response.Content.ReadAsAsync<IUser>();
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

        // Used to create a new user, used in registration
        public async Task PostUser(ErrorMessage errorMessage, User user)
        {
            try
            {
                var http = new HttpClient();

                var response = await http.PostAsJsonAsync($"{_baseAddress}/User", user);
                if (CheckStatusCodes(response, errorMessage))
                    return;
            }            
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return;
            }
        }

        // Updates user info, used in settings page
        public async Task PutUser(ErrorMessage errorMessage, User user)
        {
            try
            {
                var http = new HttpClient();

                var response = await http.PutAsJsonAsync($"{_baseAddress}/User", user);
                if (CheckStatusCodes(response, errorMessage))
                    return;
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return;
            }
        }
    }
}
