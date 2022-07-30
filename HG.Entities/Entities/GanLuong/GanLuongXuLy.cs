using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.GanLuongXuLy
{
    [Table("gan_luong_xu_ly")]
    public class Gan_Luong_Xu_Ly : BaseDanhMucModel
    {
        public Guid? Id { get; set; }
        public string? ma_gan_luong { get; set; }
        public string? ten_gan_luong { get; set; }
        public string? mo_ta { get; set; }
        public string mac_dinh { get; set; }
        public string? ma_phong_ban { get; set; }
        public string? ma_linh_vuc { get; set; }
        public string? ma_dich_vu { get; set; }
        public string? ma_luong_xu_ly { get; set; }
        public int? so_thu_tu { get; set; }
       

    }
    public class GanLuongXuLy_paging
    {
        public string ma_gan_luong { get; set; }
        public string ten_gan_luong { get; set; }
        public string mo_ta { get; set; }
       
        public List<Gan_Luong_Xu_Ly> lstGanLuongXuLy { get; set; }
       
        public Gan_Luong_Xu_Ly ganLuongXuLy { get; set; }
        public Pagelist Pagelist { get; set; } = new Pagelist();
    }
    public class Asp_GanLuong_XuLy
    {
        public Gan_Luong_Xu_Ly ganLuongXuLyModel { get; set; }
        //  public List<Asp_nhom> asp_nhom { get; set; }
        public Response? responseErr { get; set; }
    }
    
}
