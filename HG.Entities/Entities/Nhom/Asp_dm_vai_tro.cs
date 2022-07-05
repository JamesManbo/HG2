using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.Nhom
{
    [Table("Asp_dm_vai_tro")]
    public class Asp_dm_vai_tro : BaseModel
    {
        public string ma_vai_tro { get; set; }
        public string? buoc_xu_ly { get; set; }
        public string? ten_vai_tro { get; set; }
        public string? mo_ta { get; set; }
        public int? stt { get; set; }
    }
}
