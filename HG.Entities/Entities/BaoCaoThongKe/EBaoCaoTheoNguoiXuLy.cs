namespace HG.Entities.BaoCaoThongKe
{
    public class EBaoCaoTheoNguoiXuLy
    {
        public long Id { get; set; }
        public Guid IdNguoiXuLy { get; set; }
        public string TenNguoiXuLy { get; set; }


        public string IdPhongBan { get; set; }
        public string TenPhongBan { get; set; }


        public int ChuaXuLyDungHan { get; set; }
        public int ChuaXuLyQuaHan { get; set; }
        public int DangXuLyDungHan { get; set; }
        public int DangXuLyQuaHan { get; set; }
        public int DaXuLyDungHan { get; set; }
        public int DaXuLyQuahan { get; set; }
        public int Tong { get; set; }
        public DateTime NgayTongHop { get; set; }
        public long NgayTongHopInt { get; set; }

    }
}
