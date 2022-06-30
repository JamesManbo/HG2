using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.Luong
{
    public class QuyTrinhXuLy : BaseDanhMucModel
    {
        public int Id { get; set; }
        public string ma_luong { get; set; }
        public string ma_buoc { get; set; }
        public string ten_buoc { get; set; }
        public float so_ngay_xl { get; set; }
        public bool buoc_xl_chinh { get; set; }
        public string? chuc_nang { get; set; }
        public Guid nguoi_xl_mac_dinh { get; set; }
        public int? stt { get; set; }
        public string? ma_nhanh { get; set; }
        public string? nguoi_co_the_xl { get; set; }
        public string? ten_nhanh { get; set; }
        public string? nguoi_phoi_hop_xl { get; set; }

    }

    public class NhanhXuLy
    {
        public string ma_nhanh { get; set; }
        public string ten_nhanh { get; set; }
    }
    public class QuyTrinhXuLy_paging
    {
        public string ma_buoc { get; set; }
        public string ma_luong { get; set; }
        public List<QuyTrinhXuLy>? lstQuyTrinhXuLy { get; set; }
        public List<NhanhXuLy>? lstNhanhXuLy { get; set; }
        public QuyTrinhXuLy quyTrinhXuLy { get; set; }
        public Pagelist Pagelist { get; set; } = new Pagelist();
    }
}
