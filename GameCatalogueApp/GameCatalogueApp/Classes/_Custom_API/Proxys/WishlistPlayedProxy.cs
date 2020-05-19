using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes._Custom_API.Proxys
{
    public class WishlistPlayedProxy : IWishlistPlayedProxy
    {
        private readonly string _baseAddress;

        public delegate void ErrorMessage(string message);

        public WishlistPlayedProxy(string baseAddress)
        {
            _baseAddress = baseAddress;
        }

        // This is a generic class that handles both the Played API calls and the Wishlist Calls
        // As the API controllers are both the same this class can be generic to cut down the amount of API Proxys in the project

        // Checks the status codes for the HttpResponse and returns true or false and a error message if needed
        private bool CheckStatusCodes(HttpResponseMessage response, ErrorMessage errorMessage)
        {
            // This handles all status errors
            if (response.IsSuccessStatusCode)
                return true;
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) // If the API data is somehow null
            {
                errorMessage($"Oops, Try again later, something went wrong: {response.ReasonPhrase}");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound) // If the amount of items retrieved is 0 (No games were found in the database)
            {
                errorMessage("No Items were found, Try again later");
            }
            else // Any unprepared status code
            {
                errorMessage($"Something went wrong! \nStatus Code: {response.StatusCode} \nError Message{response.ReasonPhrase}");
            }
            return false;
        }

        // Gets an item associated with the user ID
        public async Task<List<T>> GetItems<T>(ErrorMessage errorMessage, string itemChoice, string id)
        {
            try
            {
                if (itemChoice.ToLower() == "wishlist" || itemChoice.ToLower() == "played")
                {

                    var http = new HttpClient
                    {
                        BaseAddress = new Uri(_baseAddress)
                    };

                    var url = $"{itemChoice}/id={id}";
                    HttpResponseMessage response = http.GetAsync(url).Result;
                    if (CheckStatusCodes(response, errorMessage))
                    {
                        var games = response.Content.ReadAsAsync<List<T>>();
                        return await games;
                    }
                    else
                        return null;
                }
                else
                {
                    errorMessage($"Unable to retrieve: {itemChoice}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return null;
            }
        }

        public async Task PostItem<T>(ErrorMessage errorMessage, T item, string itemChoice)
        {
            try
            {
                if (itemChoice.ToLower() == "played" || itemChoice.ToLower() == "wishlist")
                {
                    var http = new HttpClient();

                    HttpResponseMessage response = await http.PostAsJsonAsync($"{_baseAddress}/{itemChoice}", item);
                    if (CheckStatusCodes(response, errorMessage))
                        return;
                }
                else
                {
                    errorMessage($"Unable to Add to {itemChoice}");
                    return;
                }

            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return;
            }
        }

        public async Task DeleteItem(ErrorMessage errorMessage, string itemChoice, string id)
        {
            try
            {
                if (itemChoice.ToLower() == "played" || itemChoice.ToLower() == "wishlist")
                {
                    var http = new HttpClient();

                    HttpResponseMessage response = await http.DeleteAsync($"{_baseAddress}/{id}");
                    if (CheckStatusCodes(response, errorMessage))
                        return;
                }
                else
                {
                    errorMessage($"Unable to delete from {itemChoice}");
                    return;
                }

            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return;
            }
        }


    }
}
