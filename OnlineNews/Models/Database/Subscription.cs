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
        public bool PaymentComplete { get; set; }//Here i need to write paid/Not paid later in the project to appear on users screen
        public   User Subscriber { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
    }
}
