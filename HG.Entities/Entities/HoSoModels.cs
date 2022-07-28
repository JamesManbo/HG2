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
        public string nguoi_xu_ly { get; set; }
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
        public string lstThanhPhan { get; set; } //thành phần đã tồn tại ở tthc


    }
    public class TP_1
    {
        public string ThanhPhan_1 { get; set; }
        public int? BanSao1 { get; set; }
        public int? BanChinh1 { get; set; }
        public int? BanPhoto1 { get; set; }
        public string file_dinh_kem1 { get; set; }
    }
    public class TP_2
    {
        public string ThanhPhan_2 { get; set; }
        public int? BanSao2 { get; set; }
        public int? BanChinh2 { get; set; }
        public int? BanPhoto2 { get; set; }
        public string file_dinh_kem2 { get; set; }
    }
    public class TP_3
    {
        public string ThanhPhan_3 { get; set; }
        public int? BanSao3 { get; set; }
        public int? BanChinh3 { get; set; }
        public int? BanPhoto3 { get; set; }
        public string file_dinh_kem3 { get; set; }
    }
    public class TP_4
    {
        public string ThanhPhan_4 { get; set; }
        public int? BanSao4 { get; set; }
        public int? BanChinh4 { get; set; }
        public int? BanPhoto4 { get; set; }
        public string file_dinh_kem4 { get; set; }
    }
    public class TP_5
    {
        public string ThanhPhan_5 { get; set; }
        public int? BanSao5 { get; set; }
        public int? BanChinh5 { get; set; }
        public int? BanPhoto5 { get; set; }
        public string file_dinh_kem5 { get; set; }
    }

}
