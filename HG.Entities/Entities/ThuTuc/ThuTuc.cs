using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.ThuTuc
{
    public class DmThuTuc
    {
        public int ma_thu_tuc_old { get; set; }
        public string ma_thu_tuc { get; set; }
        public string ma_quoc_gia { get; set; }
        public string ten_thu_tuc { get; set; }
        public string mo_ta { get; set; }
        public bool nop_online { get; set; }
        public bool hd_online { get; set; }
        public bool tthcc { get; set; }
        public bool tthtks { get; set; }
        public string ma_linh_vuc { get; set; }
        public string ten_linh_vuc { get; set; }
        public string ma_phong_ban { get; set; }
        public int so_ngay_xl { get; set; }
        public decimal le_phi_truoc { get; set; }
        public decimal le_phi_sau { get; set; }
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

        public bool gui_lt_kem_kq_xl { get; set; }
        public bool chi_tra_kq_kckqlt { get; set; }
        public string thuc_hien { get; set; }
        public string don_vi_ltxl { get; set; }
        public string don_vi_ltph { get; set; }
        public bool thu_le_phi_kckq { get; set; }
        public Guid? CreatedUid { get; set; }
        public bool luong { get; set; }
        public string UidName { get; set; }
        public int count_tp { get; set; }
    }
    public class MapLuong
    {
        public string ma_luong { get; set; }
        public string ten_luong { get; set; }
    }
    public class ThanhPhan
    {
        public string ma_thu_tuc { get; set; }
        public string ma_thanh_phan { get; set; }
        public string ten_thanh_phan { get; set; }
        public string? mo_ta { get; set; }
        public int bat_buoc { get; set; }
        public string? file_dinh_kem { get; set; }
        public int bieu_mau { get; set; }
        public string? ten_form_nhap { get; set; }
        public string? duong_dan_form_nhap { get; set; }
        public int? ban_goc { get; set; }
        public int? ban_sao { get; set; }
        public int? ban_pho_to { get; set; }
        public int? Stt { get; set; }
        public string ngay_bat_dau { get; set; }
        public string url_file { get; set; }
        public string ngay_ket_thuc { get; set; }
        public Guid? CreatedUid { get; set; }
        public string UidName { get; set; }
        public string type_view { get; set; }
        public string type_view_tt { get; set; }
    }

    public class NhomTP
    {
        public string ma_thu_tuc { get; set; }
        public string ma_mhom { get; set; }
        public string ten_nhom { get; set; }
        public string? mo_ta { get; set; }
        public string? file_dinh_kem { get; set; }
        public int? stt { get; set; }
    }

    public class VanBanLQ
    {
        public string ma_thu_tuc { get; set; }
        public string ma_van_ban { get; set; }
        public string ten_van_ban { get; set; }
        public string? mo_ta { get; set; }
        public string? file_dinh_kem { get; set; }
        public int? stt { get; set; }
    }
    public class ThanhPhanKQXL
    {
        public string ma_thu_tuc { get; set; }
        public string ma_tp_kq { get; set; }
        public string ten_tp_kq { get; set; }
        public string? mo_ta { get; set; }
        public int? stt { get; set; }
    }

    public class ThuTucPaging
    {
        public List<DmThuTuc> lstThuTuc { get; set; }
        public Pagelist Pagelist { get; set; } = new Pagelist();
    }
    public class ThuTucModel
    {
        public DmThuTuc thuTuc { get; set; }
        public List<MapLuong> lstLuong { get; set; }
        public List<ThanhPhan> lstThanhPhan { get; set; }
        public PagelistMain PagelistThanhPhan { get; set; } = new PagelistMain();
        public List<VanBanLQ> lstVanBanLQ { get; set; }
        public PagelistMain PagelistVanBanLQ { get; set; } = new PagelistMain();
        public List<NhomTP> lstNhomTP { get; set; }
        public PagelistMain PagelistNhomTP { get; set; } = new PagelistMain();
        public List<ThanhPhanKQXL> lstThanhPhanKQXL { get; set; }
        public PagelistMain PagelisthanhPhanKQXL { get; set; } = new PagelistMain();
    }
}
