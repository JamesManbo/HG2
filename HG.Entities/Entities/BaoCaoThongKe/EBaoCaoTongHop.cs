using System;

namespace HG.Entities.BaoCaoThongKe
{
    public class EBaoCaoTongHop
    {
        public long Id { get; set; }
        public int IdThuTuc { get; set; }
        public string TenThuTuc { get; set; }
        public string TenDuAn { get; set; }
        public string NguoiNop { get; set; }
        public string TenDonViToChuc { get; set; }

        public Guid IdNguoiKy { get; set; }
        public string TenNguoiKy { get; set; }
        public DateTime NgayNhan { get; set; }
        public DateTime NgayKy { get; set; }
        public DateTime NgayHenTra { get; set; }
        public DateTime NgayHoanThanh { get; set; }

        public Guid IdNguoiTiepNhan { get; set; }
        public int TrangThai { get; set; }
        public string TenTrangThai { get; set; }
        public int LinhVuc { get; set; }
        public string TenLinhVuc { get; set; }

    }
}
