using OnlineNews.Models;

namespace OnlineNews.Models.ViewModels
{
    public class CustomerOrdersVM
    {
        public Customer Id { get; set; }
        public List<Order> Orders { get; set; }
    }
}
