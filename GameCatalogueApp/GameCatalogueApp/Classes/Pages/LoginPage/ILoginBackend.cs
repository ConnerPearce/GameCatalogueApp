using GameCatalogueApp.Classes._Custom_API.Data;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes.Pages.LoginPage
{
    public interface ILoginBackend
    {
        Task<IUser> GetUser(string uName, string password, LoginBackend.ErrorMessage errorMessage);
    }
}