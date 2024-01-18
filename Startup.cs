//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using EventEatsQuotify.ContextDBConfig;
//using EventEatsQuotify.Models;
//using System;
//using System.Threading.Tasks;

//namespace EventEatsQuotify
//{
//    public class Startup
//    {
//        public Startup(IConfiguration configuration)
//        {
//            Configuration = configuration;
//        }

//        public IConfiguration Configuration { get; }

//        public void ConfigureServices(IServiceCollection services)
//        {
//            services.AddControllersWithViews();

//            var dbConnection = Configuration.GetConnectionString("dbConnection");

//            services.AddDbContext<EventEatsQuotifyDBContext>(options =>
//                options.UseSqlServer(dbConnection));

//            services.AddIdentity<ApplicationUser, IdentityRole>()
//                .AddEntityFrameworkStores<EventEatsQuotifyDBContext>();

//            services.AddRazorPages();

//            // Seed roles during service configuration
//            using (var scope = services.ser.CreateScope())
//            {
//                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//                await CreateRoles(roleManager);
//            }
//        }

//        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//        {
//            if (env.IsDevelopment())
//            {
//                app.UseDeveloperExceptionPage();
//            }
//            else
//            {
//                app.UseExceptionHandler("/Home/Error");
//                app.UseHsts();
//            }

//            app.UseHttpsRedirection();
//            app.UseStaticFiles();

//            app.UseRouting();

//            app.UseAuthentication();
//            app.UseAuthorization();

//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapControllerRoute(
//                    name: "default",
//                    pattern: "{controller=Home}/{action=Index}/{id?}");
//                endpoints.MapRazorPages();
//            });
//        }

//        private async Task CreateRoles(RoleManager<IdentityRole> roleManager)
//        {
//            // Create "Vendor" role
//            await CreateRoleIfNotExists(roleManager, "Vendor");

//            // Create "Customer" role
//            await CreateRoleIfNotExists(roleManager, "Customer");
//        }

//        private async Task CreateRoleIfNotExists(RoleManager<IdentityRole> roleManager, string roleName)
//        {
//            var roleExists = await roleManager.RoleExistsAsync(roleName);
//            if (!roleExists)
//            {
//                await roleManager.CreateAsync(new IdentityRole(roleName));
//            }
//        }
//    }
//}
