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
    [Table("dm_giay_to_hop_le")]
    public class Dm_Giay_To_Hop_Le : BaseDanhMucModel
    {
        [Key]
        public string ma_giay_to { get; set; }
        public string ten_giay_to { get; set; }
        public string? mo_ta { get; set; }       
    }
}
