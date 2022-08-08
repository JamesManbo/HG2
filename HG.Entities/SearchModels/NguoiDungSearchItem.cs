using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.SearchModels
{
    public class NguoiDungSearchItem : Pagelist
    {
        public string? ma_phong_ban { get; set; }
        public string? tu_khoa { get; set; }
        public int? trang_thai { get; set; }
        public int? da_xoa { get; set; }
        public Guid? userId { get; set; }
    }
    public class NguoiDungOnlSearchItem : Pagelist
    {    
        public string? tu_khoa { get; set; }
        public int? trang_thai { get; set; }
        public int? da_xoa { get; set; }
        public Guid? userId { get; set; }
    }
    public class HoSoPaging : Pagelist{
        public string? ma_thu_tuc { get; set; }
        public string? tu_khoa { get; set; }
        public int? tat_ca { get; set; }
        public int? dung_han { get; set; }
        public int? qua_han { get; set; }
        public int? trang_thai_hs { get; set; }
    }
}
