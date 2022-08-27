using HG.Entities;
using HG.Entities.Entities.Nhom;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities
{
    public class ds_nguoi_dung
    {
        public Guid Id { get; set; }
        public string? ten_dang_nhap { get; set; }
        public string? ho_dem { get; set; }
        public string? ten { get; set; }
        public string? ten_chuc_vu { get; set; }
        public string? ten_phong_ban { get; set; }
        public int? stt { get; set; }
        public int? khoa_tai_khoan { get; set; }

    }
    public class ds_nguoi_dung_online
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? ten { get; set; }
        public string? anh_dai_dien { get; set; }
        public string? anh_cmt { get; set; }
        public string? ho_khau_tt { get; set; }
        public string? PhoneNumber { get; set; }
        public int? stt { get; set; }
        public int? khoa_tai_khoan { get; set; }

    }
    public class ds_nguoi_dung_paging : BaseModel
    {
        public List<ds_nguoi_dung>? asp_Nhoms { get; set; }
        public Pagelist Pagelist { get; set; } = new Pagelist();
    }
    public class ds_nguoi_dung_paging_online : BaseModel
    {
        public List<ds_nguoi_dung_online>? asp_Nhoms { get; set; }
        public Pagelist Pagelist { get; set; } = new Pagelist();
    }
    public class NguoiDungModels : IdentityUser<Guid>
    {
        public string? ho_dem { get; set; }
        public string? ten { get; set; }
        public string? mat_khau { get; set; }
        public string? anh_dai_dien { get; set; }
        public string? tinh_trang_hon_nhan { get; set; }
        public Guid? RoleId { get; set; }
        public string? ma_phong_ban { get; set; }
        public string? ma_chuc_vu { get; set; }
        public int? stt { get; set; }
        public DateTime? ngay_sinh { get; set; }
        public string? lstGroup { get; set; }
        public int? khoa_tai_khoan { get; set; }
        public int? IsAdministratorPB { get; set; }
        public int? IsAdministratorDV { get; set; }
        public Response? responseErr { get; set; }

        public string? type_view { get; set; }

    }
    public class phong_ban_nhom_nguoi_dung : BaseDanhMucModel
    {
        public string? ma_nhom { get; set; }
        public string? lst_ma_nguoi_dung { get; set; }
        public string? lstGroup { get; set; }
    }
    public class Asp_NguoiDung_Nhom{
        public List<AspNetUsersModel2>  aspNetUsersModel { get; set; }
        public List<Asp_nhom> asp_nhom { get; set; }
        public Response? responseErr { get; set; }
    }
    public class Asp_NguoiDung_Nhom_Onl
    {
        public List<UserOnlineModels> aspNetUsersModel { get; set; }
      //  public List<Asp_nhom> asp_nhom { get; set; }
        public Response? responseErr { get; set; }
    }
    public class AspNetUsersModel2 : IdentityUser<Guid>
    {
		public string? ho_dem { get; set; }
        public string? ten { get; set; }
        public string? mat_khau { get; set; }
        public string? anh_dai_dien { get; set; }
        public string? tinh_trang_hon_nhan { get; set; }
        public Guid? RoleId { get; set; }
        public string? ma_phong_ban { get; set; }
        public string? ma_chuc_vu { get; set; }
        public int? khoa_tai_khoan { get; set; }
        public int? IsAdministratorPB { get; set; }
        public int? IsAdministratorDV { get; set; }
        public int? stt { get; set; }
        public DateTime? ngay_sinh { get; set; }
        public int? Deleted { get; set; }
    }
    public class UserOnlineModels : IdentityUser<Guid>
    {
        public string? ten_dang_nhap { get; set; }
        public string? Email { get; set; }
        public string? ten { get; set; }
        public string? mat_khau { get; set; }
        public string? anh_dai_dien { get; set; }
        public string? anh_cmt { get; set; }
        public string? ho_khau_tt { get; set; }
        public int? khoa_tai_khoan { get; set; }
        public string? type_view { get; set; }
       
    }
    public class Nhom_Vaitro
    {
        public string ma_nhom { get; set; } = "";
        public string ma_vai_tro { get; set; } = "";
    }
    public class NguoiDungCongDan : IdentityUser<Guid>
    {
        public string? ho_dem { get; set; }
        public string? ten { get; set; }
        public string? mat_khau { get; set; }
        public string? anh_dai_dien { get; set; }
        public string? tinh_trang_hon_nhan { get; set; }
        public Guid? RoleId { get; set; }
        public string? ma_phong_ban { get; set; }
        public string? ma_chuc_vu { get; set; }
        public string? LoaiGiayToHL { get; set; }
        public string? SoGiayTo { get; set; }
        public string? TenCoQuan { get; set; }
        public string? DiaChiCoQuan { get; set; }
        public string? TinhThanh { get; set; }
        public string? QuanHuyen { get; set; }
        public string? XaPhuong { get; set; }
        public string? CodeVerify { get; set; }
        public int? stt { get; set; }
        public DateTime? ngay_sinh { get; set; }
        public string? lstGroup { get; set; }
        public int? khoa_tai_khoan { get; set; }
        public int? IsAdministrator { get; set; }
        public Response? responseErr { get; set; }

        public string? type_view { get; set; }

    }
}
