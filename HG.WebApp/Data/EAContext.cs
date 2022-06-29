using System;
using HG.Entities;
using HG.Entities.Entities.Nhom;
using HG.WebApp.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using HG.Entities.Entities.SuperAdmin;
using HG.Entities.DanhMuc.DonVi;

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
        public virtual DbSet<Dm_Ngay_Nghi> Dm_Ngay_Nghi { get; set; }
        public virtual DbSet<Dm_menu> Dm_menu { get; set; }
        public virtual DbSet<Dm_Dan_Toc> Dm_Dan_Toc { get; set; }
        public virtual DbSet<Dm_Gioi_Tinh> Dm_Gioi_Tinh { get; set; }
        public virtual DbSet<Dm_Quoc_Tich> Dm_Quoc_Tich { get; set; }
        public virtual DbSet<Dm_Luong_Xu_Ly> Dm_Luong_Xu_Ly { get; set; }
        public virtual DbSet<Dm_Ton_Giao> Dm_Ton_Giao { get; set; }
        public virtual DbSet<Dm_Buoc_Xu_Ly> Dm_Buoc_Xu_Ly { get; set; }
        public virtual DbSet<Dm_Nhanh_Xu_Ly> Dm_Nhanh_Xu_Ly { get; set; }
        public virtual DbSet<Dm_Buoc_Thuc_Hien> Dm_Buoc_Thuc_Hien { get; set; }

        // END Danh mục

        //TuanTA
        public virtual DbSet<dm_don_vi> dm_don_vi { get; set; }
        public virtual DbSet<Asp_dm_vai_tro> Asp_dm_vai_tro { get; set; }
        public virtual DbSet<Asp_vaitro_quyen> Asp_vaitro_quyen { get; set; }
        //End TuanTA
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