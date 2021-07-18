using System.ComponentModel.DataAnnotations;

namespace CSharpProject.Models
{
    public class Association
    {
        [Key]
        public int AssociationId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int WorkId { get; set; }
        public Work Work { get; set; }
    }
}