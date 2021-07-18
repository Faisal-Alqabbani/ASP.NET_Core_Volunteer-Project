using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSharpProject.Models
{
    public class Organization
    {
        [Key]
        public int OrganizationId { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Organization Name must be more than 2 characters")]
        [Display(Name ="Organization Name")]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public int Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [NotMapped]
        [Compare("password")]
        [Display(Name = "Confirm Password")]
        public string Confirm { get; set; }
        [Required]
        [Display(Name = "Founding Date")]
        [FutureDate]
        public DateTime FoundingDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<Work> Projects { get; set; }

    }
}