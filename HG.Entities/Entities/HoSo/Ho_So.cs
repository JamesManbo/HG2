using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.HoSo
{
    [Table("ho_so")]
    public class Ho_So : BaseEntities
    {
        public string ma_khach_hang { get; set; }
        public string ho_ten { get; set; }
        public DateTime ngay_sinh { get; set; }
        public int? nam_sinh { get; set; }
        public int gioi_tinh { get; set; }
        public int? id_giay_to { get; set; }
        public string? so_giay_to { get; set; }
        public string dien_thoai { get; set; }
        public string email { get; set; }
        public string? ten_to_chuc { get; set; }
        public string dia_chi { get; set; }
        public string ten_ho_so { get; set; }
        public string ma_linh_vuc { get; set; }
        //public string ten_lich_vuc { get; set; }
        public string ma_thu_tuc_hc { get; set; }
        public string? ma_thu_tuc_hc_old { get; set; }
        public string? ten_thu_tuc_hc { get; set; }
        public string ma_luong_xu_ly { get; set; }
        public string? ten_luong_xu_ly { get; set; }
        public string? ghi_chu { get; set; }
        public string nguoi_xu_ly { get; set; }
        public string? ten_nguoi_xu_ly { get; set; }
        public string? nguoi_phoi_hop { get; set; }
        public DateTime? gio_hen_tra { get; set; }
        public DateTime ngay_hen_tra { get; set; }
        public string? id_file_dinh_kem { get; set; }
        public string? y_kien { get; set; }
        public string? lstThanhPhan { get; set; }
        public int? id_trang_thai_hs { get; set; }
        public int? id_trang_thai_xl { get; set; }
        public int? id_trang_thai_xl_detail { get; set; }
        public string? ma_co_quan { get; set; } //ma don vi or phong ban tuy thuoc doi tuong ho so
        public int? nhan_kq_truc_tuyen { get; set; }
        public int? nhan_kq_bo_phan_mot_cua { get; set; }
        public int? nhan_kq_qua_buu_chinh { get; set; }
        public int? ho_so_chua_du_dk_tiep_nhan_chinh_thuc { get; set; }
        public int? nhan_kq_zalo { get; set; }
        public int? nhan_kq_email { get; set; }
        public Decimal? le_phi { get; set; }
        public int? da_thu_phi { get; set; }
        public int? Stt { get; set; }
        public int trang_thai { get; set; }
        public int type { get; set; }
      
    }
    public class Ho_SoPaging
    {
        public int Id { get; set; }
        public string? ma_thu_tuc_hc { get; set; }
        public string? ho_ten { get; set; }
        public string? nguoi_xu_ly { get; set; }
        public DateTime? ngay_hen_tra { get; set; }
    }
}
