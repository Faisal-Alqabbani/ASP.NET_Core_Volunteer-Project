using Microsoft.EntityFrameworkCore;

namespace CSharpProject.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options){}
    
        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Work> Works {get;set;}
        public DbSet<ContactUs> ContactUss {get;set;}
        public DbSet<Association> Associations { get; set;}

    }
}