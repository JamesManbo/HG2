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
    [Table("dm_linh_vuc")]
    public class Dm_Linh_Vuc : BaseDanhMucModel
    {
        [Key]
        public string ma_linh_vuc { get; set; }
        public string ten_linh_vuc { get; set; }
        public string? mo_ta { get; set; }
       
    }
}
