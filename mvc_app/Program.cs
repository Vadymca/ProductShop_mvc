using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using mvc_app.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Threading.Tasks;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add service products
        builder.Services.AddScoped<IServiceProducts, ServiceProducts>();

        // Add database contexts
        builder.Services.AddDbContext<ProductContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
        builder.Services.AddDbContext<UserContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        // Configure Identity
        builder.Services.AddDefaultIdentity<IdentityUser>(options =>
        {
            // Email confirmation requirement
            options.SignIn.RequireConfirmedEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 4;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredUniqueChars = 0;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<UserContext>();

        // Add Controllers with Views
        builder.Services.AddControllersWithViews();

        // Add Authorization policies
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("admin", policy => policy.RequireRole("admin"));
        });

        // Configure Authentication and JWT Bearer Token
        //builder.Services.AddAuthentication(options =>
        //{
        //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //})
        //.AddJwtBearer(options =>
        //{
        //    options.TokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateLifetime = true,
        //        ValidateIssuerSigningKey = true,
        //        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        //        ValidAudience = builder.Configuration["Jwt:Audience"],
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        //    };

        //    options.Events = new JwtBearerEvents
        //    {
        //        OnAuthenticationFailed = context =>
        //        {
        //            Console.WriteLine("Authentication failed: " + context.Exception.Message);
        //            return Task.CompletedTask;
        //        },
        //        OnTokenValidated = context =>
        //        {
        //            Console.WriteLine("Token validated: " + context.SecurityToken);
        //            return Task.CompletedTask;
        //        }
        //    };
        //});

        var app = builder.Build();

        // Middleware order
        app.UseStaticFiles();

        // Routing
        app.UseRouting();

        // Identity and Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // Route configuration
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // Start the application
        app.Run();
    }
}
