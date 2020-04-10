using GameCatalogueApp.API.Data;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes
{
    public interface ISearchBackend
    {
        Task<IGameRootObject> GetGames(string search, SearchBackend.ErrorMessage errorMessage);
    }
}