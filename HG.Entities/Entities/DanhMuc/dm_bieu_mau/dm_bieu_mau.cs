using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.DanhMuc
{
    [Table("dm_bieu_mau")]
    public class dm_bieu_mau : BaseEntities
    {
        public string ma_thu_tuc { get; set; } 
        public string ten_bieu_mau { get; set; } 
        public string url_bieu_mau { get; set; } 
        public string mo_ta { get; set; } 
    }
}
