using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OnlineNews.Models.Database
{
    public class User:IdentityUser
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name of Employee is required.")]
        [Display(Name = "Name of Employee")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "LastName of Employee is required.")]
        [Display(Name = "LastName of Employee")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "LastName of Employee is required.")]
        public DateTime DateofBirth { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    }
}
