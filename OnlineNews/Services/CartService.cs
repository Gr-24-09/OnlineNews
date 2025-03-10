using OnlineNews.Models.Helper;
using OnlineNews.Models;
using OnlineNews.Models.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using OnlineNews.Services;
using Microsoft.CodeAnalysis;

namespace OnlineNews.Services
{
    public class CartService : ICartService
    {
        public CartViewModel GetCartMovies(List<CartItem> cartItems)
        {
            var cartViewModel = new CartViewModel
            {
                CartItem = cartItems,
                SubTotalPrice = cartItems.Sum(item => item.Product.Price * item.Quantity)
            };
            return cartViewModel;
        }
        public void AddToCart(List<CartItem> cartItems, Product product)
        {
            var cartItem = cartItems.FirstOrDefault(item => item.Product.Id == product.Id);
            if (cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                cartItems.Add(new CartItem
                {
                    Product = product,
                    Quantity = 1
                });
            }
        }
        public void RemoveFromCart(List<CartItem> cartItems, int productId)
        {
            var cartItem = cartItems.FirstOrDefault(item => item.Product.Id == productId);
            if (cartItem != null)
            {
                    cartItems.Remove(cartItem);
            }
        }
        public void LowerQuantity(List<CartItem> cartItems, int productId)
        {
            var cartItem = cartItems.FirstOrDefault(item => item.Product.Id == productId);
            if (cartItem != null)
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                }
                else
                {
                    cartItems.Remove(cartItem);
                }
            }
        }
    }
}
