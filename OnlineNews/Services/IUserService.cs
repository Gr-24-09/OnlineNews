namespace OnlineNews.Service
{
    public interface IUserService
    {
        public Task<string> AddEmployee();
        public void AddRoleToEmployee(string userId);
        public void CreateRole();
    }
}
