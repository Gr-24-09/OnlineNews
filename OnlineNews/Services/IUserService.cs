using OnlineNews.Models.Database;

namespace OnlineNews.Service
{
    public interface IUserService
    {
   
        Task<User> GetUserByIdAsync(string userId);
        public Task<string> AddEmployee();

    }
}
