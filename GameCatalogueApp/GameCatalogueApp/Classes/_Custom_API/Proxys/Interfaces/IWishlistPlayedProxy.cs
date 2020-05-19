using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes._Custom_API.Proxys
{
    public interface IWishlistPlayedProxy
    {
        Task DeleteItem(WishlistPlayedProxy.ErrorMessage errorMessage, string itemChoice, string id);
        Task<List<T>> GetItems<T>(WishlistPlayedProxy.ErrorMessage errorMessage, string itemChoice, string id);
        Task PostItem<T>(WishlistPlayedProxy.ErrorMessage errorMessage, T item, string itemChoice);
    }
}