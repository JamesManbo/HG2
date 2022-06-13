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
    [Table("dm_chuc_vu")]
    public class Dm_Chuc_Vu : BaseDanhMucModel
    {
        [Key]
        public string ma_chuc_vu { get; set; }
        public string ten_chuc_vu { get; set; }
        public string? mo_ta { get; set; }       
    }
}
