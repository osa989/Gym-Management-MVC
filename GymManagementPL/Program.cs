using GymManagementBLL;
using GymManagementBLL.Services.AttachmentService;
using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Data.DataSeed;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace GymManagementPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //command related to add migrations folder path
            //Add-Migration "InitialCreate" -Project GymManagementDAL -StartupProject GymManagementPL -OutputDir "Data/Migrations"

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                //options.UseSqlServer("") // first way is by specifying path 
                //options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["DefaultConnection"]);
                //options.UseSqlServer(builder.Configuration["ConnectionStrings.DefaultConnection"]);
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            //builder.Services.AddScoped<GenericRepository<Member>, GenericRepository<Member>>();
            //builder.Services.AddScoped<GenericRepository<Trainer>, GenericRepository<Trainer>>();
            //builder.Services.AddScoped<GenericRepository<Plan>, GenericRepository<Plan>>();
            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //builder.Services.AddScoped<IPlanRepository,PlanRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();  // when you need a class implent this interface create object from it 
             builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddAutoMapper(X=>X.AddProfile(new MappingProfile()));
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IPlanService,PlanService>();
            builder.Services.AddScoped<ISessionService,SessionService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddScoped(typeof(IMemberShipRepository), typeof(SessionRepository));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Config =>
            {
                //Config.Password.RequiredLength = 6;
                //Config.Password.RequireLowercase = true;
                //Config.Password.RequireUppercase = true; by default they are true
                Config.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<GymDbContext>(); // to tell identity to use this context for storing users and roles info

            builder.Services.ConfigureApplicationCookie(Config =>
            {
                Config.LoginPath = "/Account/Login"; // redirect to this path if try to access any protected resource without login
                Config.AccessDeniedPath = "/Account/AccessDenied"; // when user try to access resource not authorized to access it 
            });
            //builder.Services.AddIdentityCore<ApplicationUser>()
            //    .AddEntityFrameworkStores<GymDbContext>(); //simpler than AddIdentity - used in APIs
            builder.Services.AddScoped<IAccountService, AccountService>();


            var app = builder.Build();
            #region Seed Data - Migrate DB
            using var Scoped = app.Services.CreateScope();
            var dbContext = Scoped.ServiceProvider.GetRequiredService<GymDbContext>();
            var roleManager = Scoped.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = Scoped.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var PendingMigrations = dbContext.Database.GetPendingMigrations();
            if (PendingMigrations?.Any() ?? false) dbContext.Database.Migrate(); 
            
            GymDbContextDataSeeding.SeedData(dbContext); 
            IdetityDbContextSeeding.SeedData(roleManager,userManager);
            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            //app.MapControllerRoute(
            //    name: "Trainers",
            //    pattern: "coach/{action}",
            //    defaults: new { controller ="Trainer",action="Index"}
            //    )
            //    .WithStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run(); 
        }
    }
}
