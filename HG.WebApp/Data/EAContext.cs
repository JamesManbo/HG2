using System;
using HG.Entities;
using HG.Entities.Entities.Nhom;
using HG.WebApp.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HG.WebApp.Data
{
    public class EAContext : IdentityDbContext<AspNetUsers, AspNetRoles, Guid>
    {
        public static IConfiguration Configuration { get; private set; }
        public EAContext()
        {
        }

        public EAContext(DbContextOptions<EAContext> options)
           : base(options)
        {
        }


        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<Asp_nhom> Asp_nhom { get; set; }
        public virtual DbSet<Dm_Phong_Ban> Dm_Phong_Ban { get; set; }
        public virtual DbSet<Dm_Linh_Vuc> Dm_Linh_Vuc { get; set; }
        public virtual DbSet<Dm_So_Ho_So> Dm_So_Ho_So { get; set; }
        public virtual DbSet<Dm_Giay_To_Hop_Le> Dm_Giay_To_Hop_Le { get; set; }
        public virtual DbSet<Dm_Chuc_Vu> Dm_Chuc_Vu { get; set; }
        // END Danh mục
       
        public virtual DbSet<AspNetRoleModules> AspNetRoleModules { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("EAContext"));
                //optionsBuilder.UseSqlServer("Data Source=WIN-20421CI14U0\\SQLEXPRESS;Initial Catalog=EA; User ID=sa;Password=abcABC123!@#");
            }
        }

    }
}