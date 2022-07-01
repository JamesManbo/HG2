using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities
{
    public class Nguoi_PHXLModel : BaseDanhMucModel
    {
        public string ma_luong { get; set; }
        public string ten_luong { get; set; }
        public string? mo_ta { get; set; }
        public bool tt_hai_gd { get; set; }
        public string? file_excel { get; set; }
        public string? ma_thu_tuc { get; set; }
        public string? nguoi_xl_mac_dinh { get; set; }
        public string? nguoi_co_the_xl { get; set; }
        public string? nguoi_phoi_hop_xl { get; set; }

    }
}
