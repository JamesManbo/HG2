namespace HG.Entities.BaoCaoThongKe
{
    public class EBaoCaoHoSoBiLoai
    {
        public long Id { get; set; }
        public Guid IdNguoiXuLy { get; set; }
        public string TenNguoIXuLy { get; set; }
        public Guid IdNguoiTiepNhan { get; set; }
        public string TenNguoiTiepNhan { get; set; }
        public string IdPhongBan { get; set; }
        public string TenPhongBan { get; set; }
        public DateTime NgayNhan { get; set; }
        public long NgayNhanInt { get; set; }
        public string MaHoSo { get; set; }
        public string MaThuTuc { get; set; }
        public string TenThuTuc { get; set; }
        public string NoiDungXuLy { get; set; }
    }
}
