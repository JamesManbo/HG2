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
    [Table("dm_kenh_tin")]
    public class Dm_Kenh_Tin : BaseEntities
    {
        public string? ten_danh_muc { get; set; }
        public bool? is_hien_thi { get; set; }
        public int? Stt { get; set; }      
        public int? Status { get; set; }    
    }  
}
