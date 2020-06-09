using GameCatalogueApp.API.Data;
using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Pages.Home;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes
{
    public interface ISearchBackend
    {
        Task<IGameRootObject> GetGames(string search, HomePage.ErrorHandling errorMessage);
        Task<List<Game>> GetCustomGames(string search, HomePage.ErrorHandling errorMessage);
    }
}