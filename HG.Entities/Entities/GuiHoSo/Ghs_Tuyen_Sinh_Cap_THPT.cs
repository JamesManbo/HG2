using HG.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities
{
    [Table("ghs_tuyen_sinh_cap_thpt")]
    public class Ghs_Tuyen_Sinh_Cap_THPT : BaseDanhMucModel
    {
        [Key]
        public int Id { get; set; }
        public string? ma_ho_so { get; set; }
        public string? nam_hoc { get; set; }
        public string thong_tin_chi_tiet { get; set; }
        public int? id_truong { get; set; }
        public string? ho_ten_hoc_sinh { get; set; }
        public string? so_dinh_danh { get; set; }
        public DateTime? ngay_sinh { get; set; }

        // 1 nam, 2 nũ, 3 khác
        public string? ma_gioi_tinh { get; set; }
        public string? ma_dan_toc { get; set; }
        public string? ma_dia_ban_tinh_noi_sinh { get; set; }
        public string? ma_dia_ban_huyen_noi_sinh { get; set; }
        public string? ma_dia_ban_tinh_noi_cu_tru { get; set; }
        public string? ma_dia_ban_huyen_noi_cu_tru { get; set; }
        public string? ma_dia_ban_xa_noi_cu_tru { get; set; }
        public string? thon_xom { get; set; }
        public string? ho_ten_lien_he { get; set; }
        public string? so_cccd_lien_he { get; set; }
        public string? dien_thoai_lien_he { get; set; }
        public int? Stt { get; set; }
        // addd
        public string? ten_truong { get; set; }
        public string? ma_uu_tien { get; set; }
        public string? dat_giai { get; set; }
        public decimal? diem_khuyen_khich_dat_giai { get; set; }
        public decimal? diem_uu_tien { get; set; }
        public string? giai_khac { get; set; }
        public decimal? diem_khuyen_khich_giai_khac { get; set; }
        public decimal? lop_6_kq_hl { get; set; }
        public string? lop_6_kq_hk { get; set; }
        public decimal? lop_7_kq_hl { get; set; }
        public string? lop_7_kq_hk { get; set; }
        public decimal? lop_8_kq_hl { get; set; }
        public string? lop_8_kq_hk { get; set; }
        public decimal? lop_9_kq_hl { get; set; }
        public string? lop_9_kq_hk { get; set; }
        public string? nv1_ma_nhom { get; set; }
        public string? nv1_ma_mon_khtn { get; set; }
        public string? nv1_ma_mon_cn { get; set; }
        public string? nv12_ma_nhom { get; set; }
        public string? nv12_ma_mon_khtn { get; set; }
        public string? nv12_ma_mon_cn { get; set; }
        public string? nv2_ma_nguyen_vong { get; set; }
    }
}
