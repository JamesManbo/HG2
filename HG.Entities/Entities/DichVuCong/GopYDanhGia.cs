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
        public string? noi_dung { get; set; }
        public int Type { get; set; } //0 gop y, 1 danh gia
    }
}
