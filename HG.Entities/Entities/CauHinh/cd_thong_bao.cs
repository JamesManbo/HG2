using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.CauHinh
{
    public class cd_thong_bao : BaseDanhMucModel
    {
        [Key]
        public int id { get; set; }
        public int? ma_cha { get; set; }
        public string? ten_ma { get; set; }
        public string? link { get; set; }
        public string? noi_dung { get; set; }

    }
}
