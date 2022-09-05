namespace HG.WebApp.Models.BaoCaoThongKe
{
    public class BaoCaoTheoNguoiXuLyRequestModel
    {
        public string MaPhongBan { get; set; } = "";
        public string MaNguoiXuLy { get; set; } = "";
        public string MaLinhVuc { get; set; } = "";
        public string MaThuTuc { get; set; } = "";

        public string TuNgayStr { get; set; }
        public string DenNgayStr { get; set; }
        public DateTime TuNgay { get; set; } = DateTime.Now;
        public DateTime DenNgay { get; set; } = DateTime.Now;


    }
}
