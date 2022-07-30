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
    [Table("dm_don_vi_lien_thong")]
    public class Dm_Don_Vi_Lien_Thong : BaseDanhMucModel
    {
        [Key]
        public string ma_don_vi { get; set; }
        public string? ten_don_vi { get; set; }
        public string? Ip { get; set; }       
    }
}
