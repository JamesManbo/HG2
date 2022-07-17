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
    [Table("dm_nhanh_xu_ly")]
    public class Dm_Nhanh : BaseDanhMucModel
    {
        [Key]
        public string ma_nhanh { get; set; }
        public string ten_nhanh { get; set; }
        public string? ma_luong { get; set; }      

    }
}
