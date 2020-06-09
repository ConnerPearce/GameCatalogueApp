using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Pages.Home;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes.Pages.WishlistPlayedPage
{
    public interface IWishlistPlayedBackend
    {
        Task<List<Game>> GetPlayed(HomePage.ErrorHandling errorMessage, string id);
        Task<List<Game>> GetWishlist(HomePage.ErrorHandling errorMessage, string id);
    }
}