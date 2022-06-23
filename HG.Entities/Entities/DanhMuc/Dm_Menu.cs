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
    [Table("dm_menu_chinh")]
    public class Dm_menu : BaseDanhMucModel
    {
        [Key]
        public string ma_trang { get; set; }
        public string ten_trang { get; set; }
        public string? mo_ta { get; set; }
        public string? ma_trang_cha { get; set; }
        public int level { get; set; }
    }
}
