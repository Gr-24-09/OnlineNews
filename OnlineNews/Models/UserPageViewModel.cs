using OnlineNews.Models.Database;

namespace OnlineNews.Models.ViewModels
{
    public class UserPageViewModel
    {
        public User User { get; set; }
        public Subscription Subscription { get; set; }
        public int RemainingDays()
        {
            if (Subscription != null && Subscription.ExpiredAt > DateTime.UtcNow)
            {
                return (Subscription.ExpiredAt - DateTime.UtcNow).Days;
            }
            return 0;
        }
    }
}
