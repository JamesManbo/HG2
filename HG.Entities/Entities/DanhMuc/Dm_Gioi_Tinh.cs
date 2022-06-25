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
    [Table("dm_gioi_tinh")]
    public class Dm_Gioi_Tinh : BaseDanhMucModel
    {
        [Key]
        public string ma_gioi_tinh { get; set; }
        public string ten_gioi_tinh { get; set; }
        public string? mo_ta { get; set; }      
        
    }  
}
