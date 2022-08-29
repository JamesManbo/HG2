using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities
{
    public class UyQuyenXuLyModel : BaseDanhMucModel
    {
        public Guid? Id { get; set; }
        public Guid? Id_nguoi_uy_quyen { get; set; }
        public Guid? Id_nguoi_duoc_uy_quyen { get; set; }
        public string? Id_thu_tuc_hc { get; set; }
        public DateTime? Tu_ngay { get; set; }
        public DateTime? Den_ngay { get; set; }
       

    }
    public class ListUyQuyenModel
    {
        public Guid? Id { get; set; }
        public string? Nguoi_duoc_uy_quyen { get; set; }
        
    }
    public class Danh_sach_uy_quyen
    {
        public List<ListUyQuyenModel>? listUyQuyen { get; set; }
        public Pagelist Pagelist { get; set; } = new Pagelist();
    }
}
