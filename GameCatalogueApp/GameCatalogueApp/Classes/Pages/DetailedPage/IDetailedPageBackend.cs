using GameCatalogueApp.Classes.API.Data;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes.Pages.DetailedPage
{
    public interface IDetailedPageBackend
    {
        Task<ISingleGameRootObject> GetGame(string slug, DetailedPageBackend.ErrorMessage errorMessage);
    }
}