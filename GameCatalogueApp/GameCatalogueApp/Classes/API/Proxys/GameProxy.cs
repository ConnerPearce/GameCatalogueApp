using GameCatalogueApp.API.Data;
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

        public GameProxy(string baseAddress)
        {
            // Injects the base address during build time, allows one spot for all information that may need changing
            _baseAddress = baseAddress;
        }

        // GET ALL GAMES
        public async Task<IGameRootObject> GetAllGameInfo()
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
                var games = response.Content.ReadAsAsync<IGameRootObject>();
                return await games;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("There was an error", response.ReasonPhrase, "Ok");
                return null;
            }
        }


        // GAME SEARCH PROXY
        public async Task<IGameRootObject> GetGameBySearch(string search)
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
                var games = response.Content.ReadAsAsync<IGameRootObject>();
                return await games;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("There was an error", response.ReasonPhrase, "Ok");
                return null;
            }

        }
    }


}
