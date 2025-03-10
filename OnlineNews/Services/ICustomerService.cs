using OnlineNews.Models;
using OnlineNews.Models.ViewModels;

namespace MovieShop.Services
{
    public interface ICustomerService
    {
        Customer GetCustomerByEmail(string emailAddress);
        bool Create(Customer customer);
        bool Update(Customer customer);
        bool Delete(int id);
        List<OrderViewModel> GetAllOrdersByCustomer(int customerId);
        List<Customer> customersList ();
    }
}
