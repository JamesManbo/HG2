using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.Luong
{
    public class PhanCongThucHienModels : BaseDanhMucModel
    {
        public Guid? Id { get; set; }
        public int Id_ho_so { get; set; }
        public string han_xu_ly { get; set; }
        public string ma_quy_trinh { get; set; }
        public Guid? Id_nguoi_nhan { get; set; }
        public Guid? Id_nguoi_phoi_hop { get; set; }
        public string file_dinh_kem { get; set; }
        public string y_kien { get; set; }
        public int trang_thai { get; set; }
    }
}
