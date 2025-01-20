using System.ComponentModel.DataAnnotations;

namespace OnlineNews.Models.Database
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name of Employee is required.")]
        [Display(Name = "Name of Employee")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName of Employee is required.")]
        [Display(Name = "LastName of Employee")]
        [StringLength(50)]
        public string LastName { get; set; }


        [Required(ErrorMessage = "LastName of Employee is required.")]
        public DateTime DateofBirth { get; set; }
       
        

    }
}
