using OnlineNews.Models.Database;

namespace OnlineNews.Service
{
    public interface ISubscriptionService
    {
        Task<Subscription> GetUserSubscriptionAsync(string userId);
      
        Task<string> GetSubscriptionTypeByNameAsync(string subscriptionType);
        Task AddSubscriptionAsync(Subscription subscription);
        Task <bool> ChangeSubscriptionTypeAsync(string userId, string subscriptionType);
    }
}