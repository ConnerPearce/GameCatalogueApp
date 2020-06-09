using GameCatalogueApp.Pages.Home;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes._Custom_API.Proxys
{
    public interface IWishlistPlayedProxy
    {
        Task<bool> DeleteItem(HomePage.ErrorHandling errorMessage, string itemChoice, string id);
        Task<List<T>> GetItems<T>(HomePage.ErrorHandling errorMessage, string itemChoice, string id);
        Task<bool> PostItem<T>(HomePage.ErrorHandling errorMessage, T item, string itemChoice);
        Task<List<T>> IsOnT<T>(string itemChoice, string id);
    }
}