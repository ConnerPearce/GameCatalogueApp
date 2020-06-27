using GameCatalogueApp.Pages.Home;
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

        public WishlistPlayedProxy(string baseAddress)
        {
            _baseAddress = baseAddress;
        }

        // This is a generic class that handles both the Played API calls and the Wishlist Calls
        // As the API controllers are both the same this class can be generic to cut down the amount of API Proxys in the project

        // Checks the status codes for the HttpResponse and returns true or false and a error message if needed
        private bool CheckStatusCodes(HttpResponseMessage response, HomePage.ErrorHandling errorMessage)
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

        // Gets an item associated with the user ID, Returns a List<T> which can either be a List<Wishlist> or a List<Played>
        public async Task<List<T>> GetItems<T>(HomePage.ErrorHandling errorMessage, string itemChoice, string id)
        {
            try
            {
                if (itemChoice.ToLower() == "wishlist" || itemChoice.ToLower() == "played")
                {

                    var http = new HttpClient
                    {
                        BaseAddress = new Uri(_baseAddress)
                    };

                    var url = $"{itemChoice}/{id}";
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
                    return null;
                }
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return null;
            }
        }

        // Same as GetItems minus the ErrorMessage, if theres no items found then thats because its not on the wishlist/played
        // This is fine as its only a check to switch buttons around
        // No need to display a popup when displaying detailed game info
        public async Task<List<T>> IsOnT<T>(string itemChoice, string id)
        {
            try
            {
                if (itemChoice.ToLower() == "wishlist" || itemChoice.ToLower() == "played")
                {

                    var http = new HttpClient
                    {
                        BaseAddress = new Uri(_baseAddress)
                    };

                    var url = $"{itemChoice}/{id}";
                    HttpResponseMessage response = http.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var items = await response.Content.ReadAsAsync<List<T>>();
                        return items;
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        // Used to post an item to Wishlist or Played Tables, Returns a bool based on success
        public async Task<bool> PostItem<T>(HomePage.ErrorHandling errorMessage, T item, string itemChoice)
        {
            try
            {
                if (itemChoice == "Played" || itemChoice == "Wishlist")
                {
                    var http = new HttpClient();

                    HttpResponseMessage response = await http.PostAsJsonAsync($"{_baseAddress}{itemChoice}", item);

                    return CheckStatusCodes(response, errorMessage);
                }
                else
                {
                    errorMessage($"Unable to Add to {itemChoice}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return false;
            }
        }

        // Used to delete an item from Wishlist or Played Tables, Returns a bool based on success
        public async Task<bool> DeleteItem(HomePage.ErrorHandling errorMessage, string itemChoice, string id)
        {
            try
            {
                if (itemChoice.ToLower() == "played" || itemChoice.ToLower() == "wishlist")
                {
                    var http = new HttpClient();

                    HttpResponseMessage response = await http.DeleteAsync($"{_baseAddress}{itemChoice}/{id}");

                    return CheckStatusCodes(response, errorMessage);
                }
                else
                {
                    errorMessage($"Unable to delete from {itemChoice}");
                    return false;
                }

            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return false;
            }
        }


    }
}
