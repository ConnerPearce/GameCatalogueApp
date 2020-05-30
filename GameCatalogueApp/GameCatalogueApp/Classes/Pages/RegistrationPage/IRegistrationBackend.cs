using GameCatalogueApp.Classes._Custom_API.Data;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes.Pages.RegistrationPage
{
    public interface IRegistrationBackend
    {
        Task<bool> RegisterUser(RegistrationBackend.ErrorMessage errorMessage, User user);
    }
}