using OnlineNews.Data;
using Microsoft.AspNetCore.Identity;
using OnlineNews.Models.Database;

namespace OnlineNews.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager ;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<string> AddEmployee()
        {
            User newEmployee = new User()
            {
                UserName = "Peter.Forsberg@gmail.com",
                Email = "Peter.Forsberg@gmail.com",
                FirstName = "Peter",
                LastName = "Forsberg",
                PhoneNumber = "1234567890",
            };
            var result = await _userManager.CreateAsync(newEmployee, "newPassword22" );
            if (result.Succeeded) 
            {
                return "Success";
            }
            return "Failure";
        }
        public void AddRoleToEmployee(string userId)
        {
            return;
        }
        public void CreateRole()
        {
            return;
        }

        



    }
}
