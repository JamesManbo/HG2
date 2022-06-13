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
    [Table("dm_ho_so")]
    public class Dm_Ho_So : BaseDanhMucModel
    {
        [Key]
        public string ma_so { get; set; }
        public string ten_so { get; set; }
        public string quyen { get; set; }
        public string ma_linh_vuc { get; set; }
        public bool hoat_dong { get; set; }
        public string? mo_ta { get; set; }
    }
}
