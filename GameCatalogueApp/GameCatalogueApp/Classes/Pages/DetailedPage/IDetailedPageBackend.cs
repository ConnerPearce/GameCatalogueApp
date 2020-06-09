using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Classes.API.Data;
using GameCatalogueApp.Pages.Home;
using System;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes.Pages.DetailedPage
{
    public interface IDetailedPageBackend
    {
        Task<ISingleGameRootObject> GetGame(string slug, HomePage.ErrorHandling errorMessage);

        Task<IGame> GetCustomGame(string id, HomePage.ErrorHandling errorMessage);

        Task<bool> AddToWishlistPlayed<T>(HomePage.ErrorHandling errorMessage, T item, string itemChoice);
        Task<bool> DeleteFromWishlistPlayed(HomePage.ErrorHandling errorMessage, string itemChoice, string id);

        Task<bool> IsOnWishlist(HomePage.ErrorHandling errorMessage, Game item, string id);
        Task<bool> IsOnPlayed(HomePage.ErrorHandling errorMessage, Game item, string id);
    }
}