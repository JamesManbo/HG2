using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities
{
    public class PhieuHenModel : BaseDanhMucModel
    {    
        public string? so_giay_to { get; set; }
        public string? ten_don_vi { get; set; }
        public string? ho_ten { get; set; }
        public string? dia_chi { get; set; }
        public string? ten_to_chuc { get; set; }
        public string? dien_thoai { get; set; }
        public string? email { get; set; }
        public string? ten_thu_tuc { get; set; }
        public string? noi_dung_hd { get; set; }
        public int? so_bo_hs { get; set; }
        public int? so_ngay_xl { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
        public string? nguoi_tiep_nhan { get; set; }
    }
}
