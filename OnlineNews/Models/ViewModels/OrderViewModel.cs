using OnlineNews.Models;

namespace OnlineNews.Models.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal Totalcost { get; set; }
        public List<ProductItemViewModel> Movies { get; set; } = new List<ProductItemViewModel>();
    }

    public class ProductItemViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;

    }
}
