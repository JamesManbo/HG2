using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.DanhMuc
{
    public class dm_thanh_phan : BaseDanhMucModel
    {
        public string ma_thu_tuc { get; set; }
        public string ma_thanh_phan { get; set; }
        public string ten_thanh_phan { get; set; }
        public string? mo_ta { get; set; }
        public int bat_buoc { get; set; }
        public string? file_dinh_kem { get; set; }
        public string url_file { get; set; }
        public int bieu_mau { get; set; }
        public string? ten_form_nhap { get; set; }
        public string? duong_dan_form_nhap { get; set; }
        public int? ban_goc { get; set; }
        public int? ban_sao { get; set; }
        public int? ban_pho_to { get; set; }
        public string? ngay_bat_dau { get; set; }
        public string? ngay_ket_thuc { get; set; }
    }
}
