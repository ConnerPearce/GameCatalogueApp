using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Pages.Home;
using GameCatalogueApp.Pages.Settings;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes.Pages.SettingsPage
{
    public interface ISettingsBackend
    {
        Task<bool> UpdateUser(User user, HomePage.ErrorHandling errorMessage);
    }
}