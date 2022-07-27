using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.CauHinh
{
    public class cd_quan_ly_hoat_dong : BaseDanhMucModel
    {
        [Key]
        public int id { get; set; }
        public string? url_file { get; set; }
        public string? file_name { get; set; }     
        public string? noi_dung { get; set; }
        [NotMapped]
        public string type_add { get; set; }

    }

    public class HoatDongModel
    {
        public List<cd_quan_ly_hoat_dong> lst_hoat_dong { get; set; }
        public cd_quan_ly_hoat_dong hoat_dong { get; set; }
        public List<cd_thong_bao> lst_thong_bao { get; set; }
        public PagelistMain PagelistHD { get; set; } = new PagelistMain();
        public PagelistMain PagelistTB { get; set; } = new PagelistMain();

    }
}
