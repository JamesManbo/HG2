using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.Luong
{
    public class HoSoLienThongModels : BaseDanhMucModel
    {
        public Guid? Id { get; set; }
        public int Id_ho_so { get; set; }
        public string? han_xu_ly { get; set; }
        public string? ma_quy_trinh { get; set; }
        public Guid? Id_nguoi_nhan { get; set; }
        public Guid? Id_nguoi_phoi_hop { get; set; }
        public string? file_dinh_kem { get; set; }
        public string? y_kien { get; set; }
        public int trang_thai { get; set; }
    }
    public class ListHoSoLienThong : BaseDanhMucModel
    {    
        public DateTime Ngay { get; set; }
        public string? Don_vi_nhan { get; set; }
        public string? Nguoi_nhan { get; set; }
        public string? Kieu_lien_thong { get; set; }
        public string? Nguoi_thuc_hien { get; set; }
        public string? y_kien { get; set; }
        public int trang_thai { get; set; }
        public int trang_thai_gui { get; set; }

    }
    public class ListKetQuaLienThong : BaseDanhMucModel
    {
        public DateTime Ngay { get; set; }
        public string? Don_vi_tra_loi { get; set; }
        public string? Noi_dung { get; set; }
        public string? File_dinh_kem { get; set; }
       

    }
}
