using HG.Entities;
using HG.Entities.Entities.Nhom;
using HG.Entities.Entities.SuperAdmin;

namespace HG.WebApp.Models.DanhMuc
{
    public class PhongBanModel
    {
        public int ma_phong_ban { get; set; }
        public string ten_phong_ban { get; set; }
        public string mo_ta { get; set; }
        public int thu_tu { get; set; }
        public string nguoi_dai_dien { get; set; }
    }
    public class PhanQuyenModel : Pagelist
    {
        public Dm_menu_paging? dm_Menu_Pagings { get; set; }
        public List<AspNetRoles>? AspNetRoles { get; set; } 
        public List<Asp_dm_vai_tro>? Asp_dm_vai_tro { get; set; }   = new List<Asp_dm_vai_tro> { };
        public List<Asp_vaitro_quyen>? Asp_vaitro_quyen  { get; set; } = new List<Asp_vaitro_quyen>();
        public int TotalPage { get; set; }
        public int RecoredFrom { get; set; }
        public int RecoredTo { get; set; }
    }
    public class MenuRoleModel
    {
        public List<AspNetRoles>? AspNetRoles { get; set; }
        public Asp_vaitro_quyen? Asp_vaitro_quyen { get; set; } = new Asp_vaitro_quyen { };
        public string? ma_vai_tro { get; set; }
        public string? ma_chuc_nang { get; set; }

    }

}