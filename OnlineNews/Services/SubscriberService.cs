
using Microsoft.EntityFrameworkCore;
using OnlineNews.Data;
using OnlineNews.Models.Database;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineNews.Service
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ApplicationDbContext _db;

        public SubscriptionService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Subscription> GetUserSubscriptionAsync(string userId)
        {
            return await _db.Subscriptions
                .Include(s => s.SubscriptionType) 
                .Where(s => s.Subscriber.Id == userId)
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task <bool> ChangeSubscriptionTypeAsync(string userId, string subscriptionType)
        {
            var subscriptionTypeObject = await _db.SubscriptionTypes
                .Where(st => st.TypeName.ToLower() == subscriptionType.ToLower())
                .FirstOrDefaultAsync();

            if (subscriptionTypeObject != null)
            {
                var subscription = await _db.Subscriptions
                    .Where(s => s.Subscriber.Id == userId)
                    .OrderByDescending(s => s.CreatedAt)
                    .FirstOrDefaultAsync();

                if (subscription != null)
                {
                    subscription.SubscriptionType = subscriptionTypeObject;
                    _db.Subscriptions.Update(subscription);
                    await _db.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }


        public async Task<string> GetSubscriptionTypeByNameAsync(string subscriptionType)
        {
            return await _db.SubscriptionTypes
                .Where(st => st.TypeName.ToLower() == subscriptionType.ToLower())
                .Select(st => st.TypeName)
                .FirstOrDefaultAsync();
        }

        public async Task AddSubscriptionAsync(Subscription subscription)
        {
            _db.Subscriptions.Add(subscription);
            await _db.SaveChangesAsync();
        }
    }
}