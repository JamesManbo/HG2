using HG.Entities.Entities.DanhMuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities
{
    public class LuongThanhPhanModels
    {
        public List<Dm_Luong_Xu_Ly> dm_Luong_Xu_Lies { get; set; }
        public List<dm_thanh_phan> dm_thanh_phan { get; set; }
    }
    public class NguoiXL
    {
        public int Id { get; set; }
        public string? nguoi_xl_mac_dinh { get; set; }
        public string? nguoi_co_the_xl { get; set; }
        public string? nguoi_phoi_hop_xl { get; set; }
    }
}
