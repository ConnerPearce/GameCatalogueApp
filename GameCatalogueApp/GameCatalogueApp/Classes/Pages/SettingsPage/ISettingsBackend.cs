using GameCatalogueApp.Classes._Custom_API.Data;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes.Pages.SettingsPage
{
    public interface ISettingsBackend
    {
        Task<bool> UpdateUser(User user, SettingsBackend.ErrorMessage errorMessage);
    }
}