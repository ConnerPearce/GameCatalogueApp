using GameCatalogueApp.API.Data;
using GameCatalogueApp.Classes.API.Data;
using GameCatalogueApp.Pages.Home;
using System.Threading.Tasks;

namespace GameCatalogueApp.API
{
    public interface IGameProxy
    {
        Task<IGameRootObject> GetAllGameInfo(HomePage.ErrorHandling errorMessage);
        Task<IGameRootObject> GetGameBySearch(string search, HomePage.ErrorHandling errorMessage);
        Task<ISingleGameRootObject> GetSinlgeGameInfo(string slug, HomePage.ErrorHandling errorMessage);

    }
}