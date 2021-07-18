using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CSharpProject.Models
{
    public class Work
    {
        [Key]
        public int WorkId { get; set; }
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
        public string Picture { get; set; }="org_Reg.svg";
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
        public List<Association> Workers { get; set; }
    }
}