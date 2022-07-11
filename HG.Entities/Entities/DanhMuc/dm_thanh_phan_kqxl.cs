using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities
{
    [Table("dm_thanh_phan_kqxl")]
    public class dm_thanh_phan_kqxl : BaseEntities
    {
        public string ma_tp_kq { get; set; }
        public string ma_thu_tuc { get; set; }
        public string ten_tp_kq { get; set; }
        public string? mo_ta { get; set; }
        public int? Stt { get; set; }
    }
}
