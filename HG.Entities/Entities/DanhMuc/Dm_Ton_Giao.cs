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
    [Table("dm_ton_giao")]
    public class Dm_Ton_Giao : BaseDanhMucModel
    {
        [Key]
        public string ma_ton_giao { get; set; }
        public string ten_ton_giao { get; set; }
        public string? mo_ta { get; set; }      
        
    }  
}
