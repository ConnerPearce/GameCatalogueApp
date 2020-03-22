using GameCatalogueApp.API.Data;
using System.Threading.Tasks;

namespace GameCatalogueApp.API
{
    public interface IGameProxy
    {
        Task<IGameRootObject> GetAllGameInfo();
        Task<IGameRootObject> GetGameBySearch(string search);
    }
}