using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.CauHinh
{
    public class Asp_cau_hinh : BaseEntities
    {
        public string ma_co_quan { get; set; }
        public string? ma_dinh_danh { get; set; }
        public string? ten_dv_su_dung { get; set; }
        public string? ten_dv_chu_quan { get; set; }
        public string? source_post_vnpost { get; set; }
        public string? source_get_vnpost { get; set; }
        public string? ma_kh_VNPost { get; set; }
        public string? Token_VNPost { get; set; }
        public string? tttc_hs_qua_mang { get; set; }
        public string? left_footer { get; set; }
        public int? cau_hinh_gui_email { get; set; }
        public string? may_chu_smtp { get; set; }
        public string? cong_smtp { get; set; }
        public string? tai_khoan_dang_nhap { get; set; }
        public string? mat_khau_dang_nhap { get; set; }
        public string? id_vnpt_pay { get; set; }
        public string? tai_khoan_nhan_tien { get; set; }
        public string? bottom_footer { get; set; }
        public string? ghi_chu { get; set; }
        public int? tra_kq_khi_tiep_nhan { get; set; }
        public int? kiem_duyet_kq_ho_so { get; set; }
        public int? su_dung_trang_online_chung { get; set; }
        public int? Stt { get; set; }
        public string? hotline { get; set; }
    }
}
