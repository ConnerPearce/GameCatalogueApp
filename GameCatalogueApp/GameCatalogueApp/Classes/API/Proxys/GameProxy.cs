using GameCatalogueApp.API.Data;
using GameCatalogueApp.Classes.API.Data;
using GameCatalogueApp.Pages.Home;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GameCatalogueApp.API
{
    public class GameProxy : IGameProxy
    {
        // USED FOR DEPENDANCY INJECTION
        private readonly string _baseAddress;

        public delegate void ErrorMessage(string message);

        public GameProxy(string baseAddress)
        {
            // Injects the base address during build time, allows one spot for all information that may need changing
            _baseAddress = baseAddress;
        }

        // GET ALL GAMES
        public async Task<IGameRootObject> GetAllGameInfo(ErrorMessage errorMessage)
        {
            try
            {
                var http = new HttpClient
                {
                    BaseAddress = new Uri(_baseAddress),
                };

                // The prebuilt API requires http headers or API requests can be blocked
                // The header just contains the app name and the version
                http.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue(AppInfo.Name, AppInfo.VersionString));

                var url = "games";
                HttpResponseMessage response = http.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {

                    //If theres a succesful response return the content
                    var games = response.Content.ReadAsAsync<GameRootObject>();
                    return await games;
                }
                else
                {

                    // This is for if there is an error
                    // The delegate will return what the error is to the main xaml and then the xaml will display a pop up detailing the error
                    errorMessage(response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return null;
            }
            
        }


        // GAME SEARCH PROXY
        // Gets games by a search
        public async Task<IGameRootObject> GetGameBySearch(string search, ErrorMessage errorMessage)
        {
            try
            {
                var http = new HttpClient
                {
                    BaseAddress = new Uri(_baseAddress)
                };

                // The prebuilt API requires http headers or API requests can be blocked
                // The header just contains the app name and the version
                http.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue(AppInfo.Name, AppInfo.VersionString));

                var url = $"games?search={search}";
                HttpResponseMessage response = http.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {

                    //If theres a succesful response return the content
                    var games = response.Content.ReadAsAsync<GameRootObject>();
                    return await games;
                }
                else
                {

                    // This is for if there is an error
                    // The delegate will return what the error is to the main xaml and then the xaml will display a pop up detailing the error
                    errorMessage(response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return null;            
            }
         
        }

        // Single Game
        // Gets a single game's information based on unique identifier called a slug
        public async Task<ISingleGameRootObject> GetSinlgeGameInfo(string slug, ErrorMessage errorMessage)
        {
            try
            {

                var http = new HttpClient
                {
                    BaseAddress = new Uri(_baseAddress)
                };

                // The prebuilt API requires http headers or API requests can be blocked
                // The header just contains the app name and the version
                http.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue(AppInfo.Name, AppInfo.VersionString));

                var url = $"games/{slug}";
                HttpResponseMessage response = http.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {

                    //If theres a succesful response return the content
                    var game = response.Content.ReadAsAsync<SingleGameRootObject>();
                    return await game;
                }
                else
                {

                    // This is for if there is an error
                    // The delegate will return what the error is to the main xaml and then the xaml will display a pop up detailing the error
                    errorMessage(response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                errorMessage(ex.Message);
                return null;
            }
        }
    }


}
