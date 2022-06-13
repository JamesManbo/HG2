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
    }
}
