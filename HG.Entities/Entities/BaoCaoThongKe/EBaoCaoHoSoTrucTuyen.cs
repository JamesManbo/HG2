using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.BaoCaoThongKe
{
    public class EBaoCaoHoSoTrucTuyen
    {
        public string MaHoSo { get; set; }
        public string TenThuTuc { get; set; }
        public string TenNguoiNop{ get; set; }
        public string TenCoQuan { get; set; }

        public Guid IdNguoiNhan { get; set; }
        public string NguoiNhan { get; set; }

        public DateTime NgayNhan{ get; set; }
        public DateTime NgayHenTra { get; set; }
        public DateTime NgayTraKetQua { get; set; }
        public string TrangThai { get; set; }
        public string MucDo { get; set; }
    }
}
