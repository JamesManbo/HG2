using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.DanhMuc.DonVi
{
    [Table("dm_dia_ban")]
    public class dm_dia_ban : BaseEntities
    {
        public string ma_dia_ban { get; set; }
        public string ma_cap { get; set; }
        public string? ma_dia_ban_cha { get; set; }
        public string? ma_dia_ban_con { get; set; }
        public string? ma_dia_ban_con_c2 { get; set; }
        public string ten_dia_ban { get; set; }
        public string? mo_ta_dia_ban { get; set; }
        public int? trang_thai { get; set; }
        public int? stt { get; set; }
    }
    [Table("dm_cap_dia_ban")]
    public class dm_cap_dia_ban
    {
        public int Id { get; set; }
        public string ma_cap { get; set; }
        public string ten_cap { get; set; }
    }

}
