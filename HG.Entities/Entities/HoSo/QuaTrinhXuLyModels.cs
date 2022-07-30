using HG.Entities.Entities.DanhMuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities
{
    public class QuaTrinhXuLyModels
    {
        public List<QuaTrinhXuLy> Qua_trinh_xu_ly { get; set; }
       // public List<dm_thanh_phan> dm_thanh_phan { get; set; }
    }
    public class QuaTrinhXuLy
    {
        public DateTime han_xu_ly { get; set; }
        public DateTime? ngay_thuc_hien { get; set; }
        public string? nguoi_xu_ly { get; set; }
        public string? trinh_tu_cong_viec { get; set; }
        public string? nguoi_nhan { get; set; }
        public string? trang_thai { get; set; }
    }
}
