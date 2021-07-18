using System;
using System.ComponentModel.DataAnnotations;
using CSharpProject.Models;
using Microsoft.AspNetCore.Http;

namespace CSharpProject.Models
{
    public class WorkViewModels
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [PastDate]
        public DateTime StartDate { get; set; }
        [Required]
        [PastDate]
        public DateTime EndDate { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public IFormFile Picture { get; set; }
        [Required]
        [Display(Name="Number of Hours needed")]
        public int NumberOfHours { get; set; }
        [Required]
        [Display(Name="Number of Volunteers needed")]
        public int NumberOfVolunteers { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Skills { get; set; }
        [Required]
        [Display(Name="Minmum Age")]
        public int MinAge { get; set;}
        [Required]
        public string Category { get; set; }
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int OrganizationId { get; set; }
        public Organization CreatedBy { get; set; }
    }
}