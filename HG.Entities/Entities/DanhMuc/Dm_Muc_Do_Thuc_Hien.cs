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
    [Table("dm_muc_do_thuc_hien")]
    public class Dm_Muc_Do_Thuc_Hien : BaseDanhMucModel
    {
        [Key]
        public string ma_thuc_hien { get; set; }
        public string ten_thuc_hien { get; set; }    
        
    }  
}
