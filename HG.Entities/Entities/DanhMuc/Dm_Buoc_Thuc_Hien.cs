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
    [Table("dm_buoc_thuc_hien")]
    public class Dm_Buoc_Thuc_Hien : BaseDanhMucModel
    {
        [Key]
        public string ma_buoc { get; set; }
        public string ten_buoc { get; set; }
        public string? mo_ta { get; set; }
        public double? so_ngay { get; set; }
    }
  
}
