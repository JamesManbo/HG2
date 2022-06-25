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
    [Table("dm_quoc_tich")]
    public class Dm_Quoc_Tich : BaseDanhMucModel
    {
        [Key]
        public string ma_quoc_tich { get; set; }
        public string ten_quoc_tich { get; set; }
        public string? mo_ta { get; set; }      
        
    }  
}
