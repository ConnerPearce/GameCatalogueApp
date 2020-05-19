using GameCatalogueApp.Classes._Custom_API.Data;
using System.Threading.Tasks;

namespace GameCatalogueApp.Classes._Custom_API.Proxys
{
    public interface IUserProxy
    {
        Task<IUser> GetUser(UserProxy.ErrorMessage errorMessage, string uName, string password);
        Task PostUser(UserProxy.ErrorMessage errorMessage, User user);

        Task PutUser(UserProxy.ErrorMessage errorMessage, User user);
    }
}