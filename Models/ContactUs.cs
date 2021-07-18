using System.ComponentModel.DataAnnotations;

namespace CSharpProject.Models
{
    public class ContactUs
    {
        [Key]
        public int ContactUsId { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Msg { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        
    }
}