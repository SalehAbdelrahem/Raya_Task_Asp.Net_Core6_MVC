using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace DAL.Data
{
    public class AppDBContext : IdentityDbContext<User,Role,int>
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("User");
            builder.Entity<Role>().ToTable("Role");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRole");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaim");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogin");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaim");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserToken");
            builder.Entity<Role>().HasData(new Role {Id=1, Name="HR" ,NormalizedName="HR"} , new Role {Id=2, Name="HR_Admin" ,NormalizedName="HR_ADMIN"});
        }
        public DbSet<Employee> Employees { get; set; }

    }
}
