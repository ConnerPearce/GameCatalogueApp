using GameCatalogueApp.Classes._Custom_API.Data;
using GameCatalogueApp.Pages.Home;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes._Custom_API.Proxys
{
    public interface IUserProxy
    {
        Task<IUser> GetUser(HomePage.ErrorHandling errorMessage, string uName, string password);
        Task<bool> PostUser(HomePage.ErrorHandling errorMessage, User user);

        Task<bool> PutUser(HomePage.ErrorHandling errorMessage, User user);
    }
}