using HG.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities
{
    [Table("dm_dan_toc")]
    public class Dm_Dan_Toc : BaseDanhMucModel
    {
        [Key]
        public string ma_dan_toc { get; set; }
        public string ten_dan_toc { get; set; }
        public string? mo_ta { get; set; }
        public string? ma_so { get; set; }
        public string? ten_goi_khac { get; set; }
    }  
}
