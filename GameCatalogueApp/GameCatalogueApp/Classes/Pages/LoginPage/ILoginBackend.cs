using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Pages.Home;
using GameCatalogueApp.Pages.Login;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes.Pages.LoginPage
{
    public interface ILoginBackend
    {
        Task<IUser> GetUser(string uName, string password, HomePage.ErrorHandling errorMessage);
    }
}