namespace OnlineNews.Service
{
    public interface IAdminService
    {

        public Task<string> AddRoleToEmployee(string userId);
        public Task<string> CreateRole();


    }
}
