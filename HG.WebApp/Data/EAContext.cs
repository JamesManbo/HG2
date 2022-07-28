using System;
using HG.Entities;
using HG.Entities.Entities.Nhom;
using HG.WebApp.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using HG.Entities.Entities.SuperAdmin;
using HG.Entities.DanhMuc.DonVi;
using HG.Entities.Entities.DanhMuc.DonVi;
using HG.Entities.CauHinh;
using HG.Entities.Entities.ThuTuc;
using HG.Entities.HoSo;
using HG.Entities.Entities.DanhMuc;
using HG.Entities.DanhMuc;
using HG.Entities.Entities.HoSo;
using HG.Entities.Entities.CauHinh;

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
        public virtual DbSet<Dm_Kenh_Tin> Dm_Kenh_Tin { get; set; }
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
        public virtual DbSet<Dm_Don_Vi_Lien_Thong> Dm_Don_Vi_Lien_Thong { get; set; }
        public virtual DbSet<Dm_Muc_Do_Thuc_Hien> Dm_Muc_Do_Thuc_Hien { get; set; }
        public virtual DbSet<cd_phien_lam_viec> cd_phien_lam_viec { get; set; }
        public virtual DbSet<cd_tt_chuyen_vien> cd_tt_chuyen_vien { get; set; }
        public virtual DbSet<cd_thong_bao> cd_thong_bao { get; set; }
        public virtual DbSet<cd_quan_ly_hoat_dong> cd_quan_ly_hoat_dong { get; set; }

        // END Danh mục

        //TuanTA
        public virtual DbSet<dm_don_vi> dm_don_vi { get; set; }
        public virtual DbSet<dm_dia_ban> dm_dia_ban { get; set; }
        public virtual DbSet<dm_cap_dia_ban> dm_cap_dia_ban { get; set; }
        public virtual DbSet<Asp_dm_vai_tro> Asp_dm_vai_tro { get; set; }
        public virtual DbSet<Asp_vaitro_quyen> Asp_vaitro_quyen { get; set; }
        public virtual DbSet<Asp_cau_hinh> Asp_cau_hinh { get; set; }
        public virtual DbSet<dm_bieu_mau> dm_bieu_mau { get; set; }
        public virtual DbSet<VanBanLQ> VanBanLQ { get; set; }
        public virtual DbSet<dm_nhom_tp> dm_nhom_tp { get; set; }
        public virtual DbSet<Ho_So> Ho_So { get; set; }
        public virtual DbSet<dm_thanh_phan> dm_thanh_phan { get; set; }
        public virtual DbSet<Ho_So_History> Ho_So_History { get; set; }
        //End TuanTA

        // AnTX Gửi hồ sơ 
        public virtual DbSet<Ghs_Tuyen_Sinh_Cap_Mam_Non> Ghs_Tuyen_Sinh_Cap_Mam_Non { get; set; }
        public virtual DbSet<Ghs_Tuyen_Sinh_Cap_Mam_Non_Hoso> Ghs_Tuyen_Sinh_Cap_Mam_Non_Hoso { get; set; }

        public virtual DbSet<Ghs_Tuyen_Sinh_Cap_Tieu_Hoc> Ghs_Tuyen_Sinh_Cap_Tieu_Hoc { get; set; }
        public virtual DbSet<Ghs_Tuyen_Sinh_Cap_Tieu_Hoc_Hoso> Ghs_Tuyen_Sinh_Cap_Tieu_Hoc_Hoso { get; set; }

        public virtual DbSet<Ghs_Tuyen_Sinh_Cap_THCS> Ghs_Tuyen_Sinh_Cap_THCS { get; set; }
        public virtual DbSet<Ghs_Tuyen_Sinh_Cap_THCS_Hoso> Ghs_Tuyen_Sinh_Cap_THCS_Hoso { get; set; }

        public virtual DbSet<Ghs_Tuyen_Sinh_Cap_THPT> Ghs_Tuyen_Sinh_Cap_THPT { get; set; }
        public virtual DbSet<Ghs_Tuyen_Sinh_Cap_THPT_Hoso> Ghs_Tuyen_Sinh_Cap_THPT_Hoso { get; set; }
        // End Antx -----
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
            }
        }

    }
}