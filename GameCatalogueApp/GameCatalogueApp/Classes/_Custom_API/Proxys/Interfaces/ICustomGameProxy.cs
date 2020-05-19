using GameCatalogueApp.Classes._Custom_API.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes._Custom_API.Proxys
{
    public interface ICustomGameProxy
    {
        Task<List<IGame>> GetAllGames(CustomGameProxy.ErrorMessage errorMessage);
        Task<IGame> GetGameByID(CustomGameProxy.ErrorMessage errorMessage, string id);
        Task<List<IGame>> GetGamesBySearch(CustomGameProxy.ErrorMessage errorMessage, string search);
    }
}