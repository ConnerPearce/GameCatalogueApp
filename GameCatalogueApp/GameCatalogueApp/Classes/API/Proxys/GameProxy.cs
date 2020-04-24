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
            var http = new HttpClient
            {
                BaseAddress = new Uri(_baseAddress),
            };
            http.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue(AppInfo.Name, AppInfo.VersionString));

            var url = "games";
            HttpResponseMessage response = http.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var games = response.Content.ReadAsAsync<GameRootObject>();
                return await games;
            }
            else
            {
                errorMessage(response.ReasonPhrase);
                return null;
            }
        }


        // GAME SEARCH PROXY
        // Gets games by a search
        public async Task<IGameRootObject> GetGameBySearch(string search, ErrorMessage errorMessage)
        {

            var http = new HttpClient
            {
                BaseAddress = new Uri(_baseAddress)
            };
            http.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue(AppInfo.Name, AppInfo.VersionString));

            var url = $"games?search={search}";
            HttpResponseMessage response = http.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var games = response.Content.ReadAsAsync<GameRootObject>();
                return await games;
            }
            else
            {
                errorMessage(response.ReasonPhrase);
                return null;
            }

        }

        // Single Game
        // Gets a single game's information based on unique identifier called a slug
        public async Task<ISingleGameRootObject> GetSinlgeGameInfo(string slug, ErrorMessage errorMessage)
        {
            var http = new HttpClient
            {
                BaseAddress = new Uri(_baseAddress)
            };
            http.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue(AppInfo.Name, AppInfo.VersionString));

            var url = $"games/{slug}";
            HttpResponseMessage response = http.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var game = response.Content.ReadAsAsync<SingleGameRootObject>();
                return await game;
            }
            else
            {
                errorMessage(response.ReasonPhrase);
                return null;
            }

        }
    }


}
