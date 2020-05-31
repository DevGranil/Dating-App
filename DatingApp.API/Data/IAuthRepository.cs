using System.Threading.Tasks;
using DatingApp.API.models;

namespace DatingApp.API.Data
{
    public interface IAuthRepository
    {

        // registering a new user. Returns a Task of type user
        Task<User> Register(User user, string password);

        // login to api
        Task<User> Login(string username, string password);

        Task<bool> UserExists(string username);

    }
}