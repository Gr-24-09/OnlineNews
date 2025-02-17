using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineNews.Data;
using OnlineNews.Models.Database;
using OnlineNews.Interfaces;
using OnlineNews.Services;
using OnlineNews.Service;
using Microsoft.AspNetCore.Identity.UI.Services;
using OnlineNews.Models.Helper;
using FluentAssertions.Common;

namespace OnlineNews
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("LexiconConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IArticleService, ArticleService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IRequestService, RequestService>();
            builder.Services.AddHttpClient<RequestService>();
            builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();


            builder.Services.AddControllersWithViews();
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();

                SeedData.Initialize(services, context);
            }
          

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[] { "Admin", "Editor", "Subscriber","Writer" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                string email = "admin@admin.com";
                string password = "Admin_password_1";

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new User();
                    user.UserName = email;
                    user.Email = email;

                    await userManager.CreateAsync(user, password);
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                string email = "madhurimandalapu82473@gmail.com";
                string password = "H@mms1992";

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new User();
                    user.UserName = email;
                    user.Email = email;
                    await userManager.CreateAsync(user, password);
                    await userManager.AddToRoleAsync(user, "Writer");
                }
            }

            app.Run();
        }
    }
}
