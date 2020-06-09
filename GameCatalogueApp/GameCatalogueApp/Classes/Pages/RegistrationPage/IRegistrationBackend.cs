using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Pages.Home;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes.Pages.RegistrationPage
{
    public interface IRegistrationBackend
    {
        Task<bool> RegisterUser(HomePage.ErrorHandling errorMessage, User user);
    }
}