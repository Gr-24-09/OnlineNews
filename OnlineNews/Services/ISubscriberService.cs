using OnlineNews.Models.Database;


namespace OnlineNews.Service
{
    public interface ISubscriptionService
    {
        Task<Subscription> GetUserSubscriptionAsync(string userId);
        Task<bool> ChangeSubscriptionTypeAsync(string userId, string subscriptionType);
        Task<string> GetSubscriptionTypeByNameAsync(string subscriptionType);
    }
}