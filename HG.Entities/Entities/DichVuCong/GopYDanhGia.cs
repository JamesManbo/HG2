using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.DichVuCong
{
    [Table("GopY")]
    public class GopYDanhGia : BaseEntities
    {
        public string ma_don_vi { get; set; }
        public string ho_va_ten { get; set; }
        public string? dia_chi { get; set; }
        public string? so_dien_thoai { get; set; }
        public string? email { get; set; }
        public string? noi_dung { get; set; } //lý do
        public int Type { get; set; } //0 gop y, 1 danh gia
        public string? ma_phong_ban { get; set; }
        public string? ma_can_bo { get; set; }
        public int? ma_ho_so { get; set; }
        public int? hai_long { get; set; }
        public int? khong_hai_long { get; set; }
    }
}
