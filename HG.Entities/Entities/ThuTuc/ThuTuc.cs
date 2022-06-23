using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.ThuTuc
{
    public class ThuTuc
    {
        public string ma_thu_tuc { get; set; }
        public string ma_quoc_gia { get; set; }
        public string ten_thu_tuc { get; set; }
        public string mo_ta { get; set; }
        public bool nop_online { get; set; }
        public bool hd_online { get; set; }
        public bool tthcc { get; set; }
        public string ma_linh_vuc { get; set; }
        public string ma_phong_ban { get; set; }
        public int so_ngay_xl { get; set; }
        public decimal le_phi { get; set; }
        public int so_bo_hs { get; set; }
        public string duong_dan_kq { get; set; }
        public string thong_tin_mo_rong { get; set; }
        public string noi_dung_hd { get; set; }
        public int stt { get; set; }
        public bool thu_le_phi_knhs { get; set; }
        public bool thuc_hien_hai_gd { get; set; }
        public bool tra_kq_ktn { get; set; }
        public bool cho_phep_lien_thong { get; set; }

        public bool cho_phep_gui_lien_thong { get; set; }
        public bool cho_phep_nhan_lien_thong { get; set; }

        public string lst_ma_dvlt_xl { get; set; }

        public string lst_ma_dvlt_ph { get; set; }
        public bool gui_lt_kem_kq_xl { get; set; }
        public bool chi_tra_kq_kckqlt { get; set; }
        

    }
}
