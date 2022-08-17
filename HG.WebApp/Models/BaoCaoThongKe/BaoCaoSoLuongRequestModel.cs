namespace HG.WebApp.Models.BaoCaoThongKe
{
    public class BaoCaoSoLuongRequestModel
    {
        public int Nam { get; set; } = DateTime.Now.Year;
        public int Quy { get; set; } = DateTime.Now.Month;
        public bool ChonNgay { get; set; } = false;

        public string TuNgayStr { get; set; }
        public string DenNgayStr { get; set; }
        public DateTime TuNgay { get; set; } = DateTime.Now;
        public DateTime DenNgay { get; set; } = DateTime.Now;


    }
}
