namespace HG.WebApp.Models.BaoCaoThongKe
{
    public class BaoCaoHoSoBiLoaiRequestModel
    {
        public string MaNguoiXuLy { get; set; }
        public int TinhTrang { get; set; }
        public string TuNgayStr { get; set; }
        public DateTime TuNgay { get; set; }
        public string DenNgayStr { get; set; }
        public DateTime DenNgay { get; set; }
        public string MaPhongBan { get; set; }
        public string MaLinhVuc { get; set; }
        public string MaThuTuc { get; set; }
    }
}
