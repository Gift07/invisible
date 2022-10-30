global using MyApplicatioon.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MyApplicatioon.Authentication;
using Microsoft.EntityFrameworkCore;

namespace MyApplicatioon.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<CustomerModel> Customer { get; set; }
    }
}