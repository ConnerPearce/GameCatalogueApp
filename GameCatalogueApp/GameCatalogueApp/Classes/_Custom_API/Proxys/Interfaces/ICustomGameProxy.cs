using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Pages.Home;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes._Custom_API.Proxys
{
    public interface ICustomGameProxy
    {
        Task<List<Game>> GetAllGames(HomePage.ErrorHandling errorMessage);
        Task<IGame> GetGameByID(HomePage.ErrorHandling errorMessage, string id);
        Task<List<Game>> GetGamesBySearch(HomePage.ErrorHandling errorMessage, string search);
    }
}