using OnlineNews.Models;
using OnlineNews.Models.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using OnlineNews.Services;



namespace OnlineNews.Services
{
    public interface ICartService
    {
        public CartViewModel GetCartMovies(List<CartItem> cartItems);
        public void AddToCart(List<CartItem> cartItems,Product product);
        public void RemoveFromCart(List<CartItem> cartItems, int productId);
        public void LowerQuantity(List<CartItem> cartItems, int productId);
    }
}
