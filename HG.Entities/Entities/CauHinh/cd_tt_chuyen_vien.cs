using HG.Entities.DanhMuc.DonVi;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.CauHinh
{
    public class cd_tt_chuyen_vien : BaseDanhMucModel
    {
        [Key]
        public int id { get; set; }
        public string? don_vi { get; set; }
        public string? ten_chuyen_vien { get; set; }
        public string? sdt { get; set; }
        public string? ma_linh_vuc { get; set; }
        public string? uid_zalo { get; set; }
        public string? ten_don_vi { get; set; }
    }

    public class CauHinhModel
    {
        public List<cd_tt_chuyen_vien> lst_chuyen_vien { get; set; }
        public cd_tt_chuyen_vien chuyen_vien { get; set; }
        public cd_phien_lam_viec phien_lv { get; set; }
        public List<dm_don_vi> lst_don_vi { get; set; }
        public List<Dm_Linh_Vuc> lst_linh_vuc { get; set; }
    }
}
