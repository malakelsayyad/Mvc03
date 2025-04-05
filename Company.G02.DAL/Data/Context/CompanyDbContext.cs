using Company.G02.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.DAL.Data.Context
{
    public class CompanyDbContext : IdentityDbContext<AppUser>
    {
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole()
            {
                Id = "C4D17A15-B5A6-41BD-9B6E-B10F86CB48EE",
                Name="Admin",
                NormalizedName="ADMIN"
            });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole()
            {
                Id = "E11E65D8-18C4-493F-AF19-4A3B80F8AC01",
                Name="User",
                NormalizedName="USER"
            });

            PasswordHasher<AppUser> passwordHasher = new PasswordHasher<AppUser>();


            modelBuilder.Entity<AppUser>().HasData(new AppUser()
            {
                Id = "4EA19E03-C84D-453C-A6B2-CF3A9981F594",
                FirstName="malak",
                LastName="elsayyad",
                Email="malakelsayyad@gmail.com",
                PasswordHash=passwordHasher.HashPassword(null,"12345Malak@")

            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>() {
              
                RoleId= "C4D17A15-B5A6-41BD-9B6E-B10F86CB48EE",
              UserId= "4EA19E03-C84D-453C-A6B2-CF3A9981F594"

            });

            base.OnModelCreating(modelBuilder);

        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.; Database=CompanyG02; Trusted_connection=True; TrustServerCertificate=True");
        //}
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        

    }
}
