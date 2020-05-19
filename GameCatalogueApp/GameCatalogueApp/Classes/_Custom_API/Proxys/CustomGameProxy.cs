using GameCatalogueApp.Classes._Custom_API.Data;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes._Custom_API.Proxys
{

    public class CustomGameProxy : ICustomGameProxy
    {
        private readonly string _baseAddress;

        public delegate void ErrorMessage(string message);

        public CustomGameProxy(string baseAddress)
        {
            _baseAddress = baseAddress;
        }

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
                errorMessage("No Games were found, Try again later");
            }
            else // Any unprepared status code
            {
                errorMessage($"Something went wrong! \nStatus Code: {response.StatusCode} \nError Message{response.ReasonPhrase}");
            }
            return false;
        }

        // Gets every game from the MongoDB Database
        public async Task<List<IGame>> GetAllGames(ErrorMessage errorMessage)
        {
            try
            {
                var http = new HttpClient
                {
                    BaseAddress = new Uri(_baseAddress)
                };

                HttpResponseMessage response = http.GetAsync("Game").Result;
                if (CheckStatusCodes(response, errorMessage))
                {
                    var games = response.Content.ReadAsAsync<List<IGame>>();
                    return await games;
                }
                else
                    return null;

            }
            catch (Exception ex) // Any exception in the program
            {
                errorMessage(ex.Message);
                return null;
            }
        }

        // Gets Games by search

        public async Task<List<IGame>> GetGamesBySearch(ErrorMessage errorMessage, string search)
        {
            try
            {
                var http = new HttpClient
                {
                    BaseAddress = new Uri(_baseAddress)
                };

                var url = $"Game/search={search}";
                HttpResponseMessage response = http.GetAsync("Game").Result;
                if (CheckStatusCodes(response, errorMessage))
                {
                    var games = response.Content.ReadAsAsync<List<IGame>>();
                    return await games;
                }
                else
                    return null;

            }
            catch (Exception ex) // Any exception in the program
            {
                errorMessage(ex.Message);
                return null;
            }
        }

        // Gets a single game by its ID
        public async Task<IGame> GetGameByID(ErrorMessage errorMessage, string id)
        {
            try
            {
                var http = new HttpClient
                {
                    BaseAddress = new Uri(_baseAddress)
                };

                var url = $"Game/id={id}";
                HttpResponseMessage response = http.GetAsync(url).Result;
                if (CheckStatusCodes(response, errorMessage))
                {
                    var games = response.Content.ReadAsAsync<IGame>();
                    return await games;
                }
                else
                    return null;

            }
            catch (Exception ex) // Any exception in the program
            {
                errorMessage(ex.Message);
                return null;
            }
        }


    }
}
