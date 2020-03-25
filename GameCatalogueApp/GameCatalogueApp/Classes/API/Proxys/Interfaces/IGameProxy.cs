using GameCatalogueApp.API.Data;
using System.Threading.Tasks;

namespace GameCatalogueApp.API
{
    public interface IGameProxy
    {
        Task<IGameRootObject> GetAllGameInfo(GameProxy.ErrorMessage errorMessage);
        Task<IGameRootObject> GetGameBySearch(string search, GameProxy.ErrorMessage errorMessage);
    }
}