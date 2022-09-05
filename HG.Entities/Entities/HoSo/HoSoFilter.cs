using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Search
{
    public class HoSoFilter
    {
        public int? ma_ho_so { get; set; }
        public string? ten_nguoi_nop { get; set; }
        public string? can_bo_tiep_nhan { get; set; }
        public string? ma_phong_ban { get; set; }
        public string? can_bo_xu_ly { get; set; }
        public int? ma_trang_thai { get; set; }
        public string? tu_ngay { get; set; }
        public string? den_ngay { get; set; }
        public string? tinh_trang { get; set; }
        public string? ma_linh_vuc { get; set; }
        public string? ma_thu_tuc_hc { get; set; }
    }
}
