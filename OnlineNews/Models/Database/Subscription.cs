using System.ComponentModel.DataAnnotations;

namespace OnlineNews.Models.Database
{
    public class Subscription
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "First Date of Subscription  is required.")]
        public DateTime CreatedAt { get; set; }

        //[Required(ErrorMessage = "Last Date of the Subscription is required.")]
        //public DateTime ExpiredAt { get; set; }
        public Boolean PaymentComplete { get; set; }//Here i need to write paid/Not paid later in the project to appear on users screen

        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<SubscriptionType> SubscriptionTypes { get; set; } = new List<SubscriptionType>();

    }
}
