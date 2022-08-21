using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.DichVuCong
{

    [Table("DanhGiaDichVu")]
    public class DanhGiaDichVu
    {
        [Key]
        public int Id { get; set; }
        public string ma_don_vi { get; set; }
        public Guid nguoi_danh_gia { get; set; }
        public int? ma_ho_so { get; set; }
        public int? CauHoi1 { get; set; }
        public int? CauHoi2 { get; set; }
        public int? CauHoi3 { get; set; }
        public int? CauHoi4 { get; set; }
        public int? CauHoi5 { get; set; }
        public int? CauHoi6 { get; set; }
        public int? CauHoi7 { get; set; }
        public int? CauHoi8 { get; set; }
        public int? CauHoi9 { get; set; }
        public int? CauHoi10 { get; set; }
        public int? CauHoi11 { get; set; }
        public int? CauHoi12 { get; set; }
        public int? CauHoi13 { get; set; }
    }
}
