using System.ComponentModel.DataAnnotations;

namespace CSharpProject.Models
{
    public class Login
    {
        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}