namespace Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();//register built-in mvc services
          

            var app = builder.Build(); 

            //app.MapGet("/", () => "Hello World!");
            //app.MapGet("/login", () => "You are signed in!");
            //app.MapGet("/login", Signin);

            app.UseStaticFiles(); // Configure midlleware static files
            //MVC
            app.MapControllerRoute(
                name:"default",
                pattern:"{controller=Home}/{action=Index}/{id?}"
                );

            app.Run();
        }
        public static string Signin() 
        {
            return $"You are signed in!";
        }
    }
}
