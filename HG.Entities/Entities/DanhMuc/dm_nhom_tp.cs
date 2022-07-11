using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities
{
    [Table("dm_nhom_tp")]
    public class dm_nhom_tp : BaseEntities
    {
        public string ma_thu_tuc { get; set; }
        public string ma_nhom { get; set; }
        public string ten_nhom { get; set; }
        public string? mo_ta { get; set; }
        public int? Stt { get; set; }

    }
}
