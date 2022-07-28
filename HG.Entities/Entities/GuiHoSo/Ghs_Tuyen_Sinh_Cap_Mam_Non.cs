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
    [Table("ghs_tuyen_sinh_cap_mam_non")]
    public class Ghs_Tuyen_Sinh_Cap_Mam_Non : BaseDanhMucModel
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
        public int? gioi_tinh { get; set; }
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
        public int? id_nguyen_vong { get; set; }
        public int? Stt { get; set; }
    }
}
