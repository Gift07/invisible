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
        public DbSet<CompanyModel> Company { get; set; }
        public DbSet<EmployeeModel> Employee { get; set; }
        public DbSet<Profits> Profit { get; set; }
        public DbSet<CustomerStages> CustomerStage { get; set; }
        public DbSet<CompainModel> Compaign { get; set; }
    }
}