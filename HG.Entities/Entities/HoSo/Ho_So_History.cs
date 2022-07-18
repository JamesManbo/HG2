using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.HoSo
{
    [Table("lich_su_ho_so")]
    public class Ho_So_History : BaseEntities
    {
        public int ma_ho_so { get; set; }
        public string noi_dung_xu_ly { get; set; }
        public int tra_thai_xu_ly_truoc { get; set; }
        public int tra_thai_xu_ly_sau { get; set; }
    }
}
