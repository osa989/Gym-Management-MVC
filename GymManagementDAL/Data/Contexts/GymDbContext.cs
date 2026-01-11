using GymManagementDAL.Data.Configuration;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Contexts
{
    public class GymDbContext : IdentityDbContext<ApplicationUser> // it also inherits from DbContext // IdentityDbContext already has DbSet for Users, Roles, UserRoles etc. // use generic version if you have custom user class
    {
        public GymDbContext(DbContextOptions<GymDbContext> options):base(options) 
        {
            
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=.;Database=GymManagement;Trusted_Connection=True;TrustServerCertificate=True;");

        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // this is important to call the base method to configure Identity tables 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // this will apply all configurations

            modelBuilder.Entity<ApplicationUser>(Eb => {
            Eb.Property(u => u.FirstName)
            .HasMaxLength(50)
            .HasColumnType("varchar");

            Eb.Property(u => u.LastName)
            .HasMaxLength(100)
            .HasColumnType("varchar");
        });
        }

        #region DbSets
        //public DbSet<ApplicationUser> Users { get; set; }

        //public DbSet<IdentityRole> Roles { get; set; }

        //public DbSet<IdentityUserRole<string>> UserRoles { get; set; }



        public DbSet<Member> Members { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<MemberShip> Memberships { get; set; }
        public DbSet<MemberSession> MemberSessions { get; set; }
        #endregion
    }
}
