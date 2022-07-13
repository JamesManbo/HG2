using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities
{
    public class HoSoModels : BaseEntities
    {
        public string ma_khach_hang { get; set; }
        public string ho_ten { get; set; }
        public DateTime ngay_sinh { get; set; }
        public int nam_sinh { get; set; }
        public int gioi_tinh { get; set; }
        public int id_giay_to { get; set; }
        public string so_giay_to { get; set; }
        public string dien_thoai { get; set; }
        public string email { get; set; }
        public string? ten_to_chuc { get; set; }
        public string dia_chi { get; set; }
        public string ten_ho_so { get; set; }
        public string ma_linh_vuc { get; set; }
        public string ten_lich_vuc { get; set; }
        public string ma_thu_tuc_hc { get; set; }
        public string ma_thu_tuc_hc_old { get; set; }
        public string ten_thu_tuc_hc { get; set; }
        public string ma_luong_xu_ly { get; set; }
        public string ten_luong_xu_ly { get; set; }
        public string ghi_chu { get; set; }
        public Guid nguoi_xu_ly { get; set; }
        public string ten_nguoi_xu_ly { get; set; }
        public string? nguoi_phoi_hop { get; set; }
        public DateTime? gio_hen_tra { get; set; }
        public DateTime ngay_hen_tra { get; set; }
        public int? id_file_dinh_kem { get; set; }
        public string? y_kien { get; set; }
        public int? id_trang_thai_hs { get; set; }
        public int? id_trang_thai_xl { get; set; }
        public int? id_trang_thai_xl_detail { get; set; }
        public string? ma_co_quan { get; set; }
        public int? nhan_kq_truc_tuyen { get; set; }
        public int? nhan_kq_bo_phan_mot_cua { get; set; }
        public int? nhan_kq_qua_buu_chinh { get; set; }
        public int? ho_so_chua_du_dk_tiep_nhan_chinh_thuc { get; set; }
        public Decimal? le_phi { get; set; }
        public int? da_thu_phi { get; set; }
        public int? Stt { get; set; }
        public int trang_thai { get; set; }
        public CLThanhPhan_1? ThanhPhan_1   { get; set; }
        public CLThanhPhan_2? ThanhPhan_2   { get; set; }

    }
    public class CLThanhPhan_1
    {
        public string ThanhPhan_1 { get; set; }
        public string? BanSao1 { get; set; }
        public string? BanChinh1 { get; set; }
        public string? BanPhoto1 { get; set; }
        public string file_dinh_kem1 { get; set; }
    }
    public class CLThanhPhan_2
    {
        public string ThanhPhan_2 { get; set; }
        public string? BanSao2 { get; set; }
        public string? BanChinh2 { get; set; }
        public string? BanPhoto2 { get; set; }
        public string file_dinh_kem2 { get; set; }
    }

}
