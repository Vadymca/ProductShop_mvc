using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mvc_app.Services;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //Add service products
        builder.Services.AddSingleton<IServiceProducts, ServiceProducts>();
        builder.Services.AddDbContext<ProductContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
        builder.Services.AddDbContext<UserContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        builder.Services.AddDefaultIdentity<IdentityUser>(options =>
        {
            //confirmed email
        options.SignIn.RequireConfirmedEmail = true;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 4;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredUniqueChars = 0;
            }).AddEntityFrameworkStores<UserContext>();
        builder.Services.AddControllersWithViews();
        var app = builder.Build();

        //identity
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseStaticFiles();
        app.UseRouting();
        //https://localhost:[port]/
        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

        app.Run();
    }
}
