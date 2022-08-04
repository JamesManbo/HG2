using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.CauHinh
{
    public class cd_theo_doi_hs
    {
        public int stt { get; set; }
        public int id { get; set; }
        public string ten_ho_so { get; set; }
        public string ma_thu_tuc_hc { get; set; }
        public DateTime ngay_tiep_nhan { get; set; }
        public DateTime ngay_hen_tra { get; set; }
        public int trang_thai { get; set; }
        public string ten_trang_thai { get; set; }
        public int so_ngay_con { get; set; }
        public string ten_don_vi { get; set; }
        public string ten_thu_tuc { get; set; }
    }

    public class ho_so_model
    {
        public List<cd_theo_doi_hs> Hs_sap_het_han { get; set; }
        public PagelistMain PagelistHs_HetHan { get; set; } = new PagelistMain();
        public PagelistMain PagelistHs_ChamTiepNhan { get; set; } = new PagelistMain();
        public List<cd_theo_doi_hs> Hs_cham_tiep_nhan { get; set; }
    }
}
