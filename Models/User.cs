using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSharpProject.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "First Name length must be more than 2 characters")]
        [MaxLength(10, ErrorMessage="Last Name length must be less than 10 characters")]
        [Display(Name="First Name")]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "First Name length must be more than 2 characters")]
        [MaxLength(10, ErrorMessage="Last Name length must be less than 10 characters")]
        [Display(Name="Last Name")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Display(Name="Phone Number")]
        public int Phone { get; set; }
        [Required]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string Confirm { get; set; }
        [Required]
        [FutureDate]
        [Display(Name = "Date of Birth")]
        public DateTime DoB {get;set;}
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        public int NumberOfHours { get; set; }=0;
        public bool IsAdmin { get; set; }=false;
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public List<Association> Works { get; set; }


    }
}