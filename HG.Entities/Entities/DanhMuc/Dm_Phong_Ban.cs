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
    [Table("dm_phong_ban")]
    public class Dm_Phong_Ban : BaseDanhMucModel
    {
        [Key]
        public string ma_phong_ban { get; set; }
        public string ten_phong_ban { get; set; }
        public string? mo_ta { get; set; }
        public Guid nguoi_dai_dien { get; set; }
       
    }
}
