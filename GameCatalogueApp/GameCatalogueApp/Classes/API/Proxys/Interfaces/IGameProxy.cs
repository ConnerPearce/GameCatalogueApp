using GameCatalogueApp.API.Data;
using GameCatalogueApp.Classes.API.Data;
using System.Threading.Tasks;

namespace GameCatalogueApp.API
{
    public interface IGameProxy
    {
        Task<IGameRootObject> GetAllGameInfo(GameProxy.ErrorMessage errorMessage);
        Task<IGameRootObject> GetGameBySearch(string search, GameProxy.ErrorMessage errorMessage);
        Task<ISingleGameRootObject> GetSinlgeGameInfo(string slug, GameProxy.ErrorMessage errorMessage);

    }
}