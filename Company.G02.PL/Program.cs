using Company.G02.BLL;
using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Data.Context;
using Company.G02.DAL.Models;
using Company.G02.PL.Mapping;
using Company.G02.PL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Company.G02.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();//Register built-in mvc services
            
            builder.Services.AddScoped<IDepartmentRepository,DepartmentRepository>();//Allows Dependancy Injection for DepartmentRepository
            builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>();//Allows Dependancy Injection for EmployeeRepository
            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();//Allows Dependancy Injection for EmployeeRepository

            builder.Services.AddDbContext<CompanyDbContext>(options => 
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });//Allows Dependancy Injection for CompanyDbContext


            //builder.Services.AddScoped(); // Create object its lifetime per request - Unreachable
            //builder.Services.AddTransient(); //Create object its lifetime per operation
            //builder.Services.AddSingleton(); //Create object its lifetime per App

            //builder.Services.AddScoped<IScopedService,ScopedService>(); // Per request
            //builder.Services.AddTransient<ITransientService,TransientService>();// Per operation
            //builder.Services.AddSingleton<ISingletonService, SingletonService>();// Per App

            //builder.Services.AddAutoMapper(typeof(EmployeeProfile));
            builder.Services.AddAutoMapper(M=>M.AddProfile(new EmployeeProfile()));
            builder.Services.AddAutoMapper(M=>M.AddProfile(new DepartmentProfile()));

            builder.Services.AddIdentity<AppUser,IdentityRole>()
                            .AddEntityFrameworkStores<CompanyDbContext>();

            builder.Services.ConfigureApplicationCookie(config => 
            {
                config.LoginPath = "/Account/SignIn";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
