using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Entities.Entities.DanhMuc
{
    public class PhongBan
    {
        public string ma_phong_ban { get; set; }
        public string ten_phong_ban { get; set; }
    }

    public class DanhSachPhongBan
    {
        public string ma_phong_ban { get; set; }
        public string ten_phong_ban { get; set; }
        public string mo_ta { get; set; }
        public string ten_nguoi_dai_dien { get; set; }
        public Guid nguoi_dai_dien { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime UpdatedDateUtc { get; set; }
        public string UpdatedUid { get; set; }
        public int Stt { get; set; }
        public string UidName { get; set; }
    }

    public class DanhSachPhongBanModel : DataResponse
    {
        public string type_view { get; set; }
        public int pageCount { get; set; }
        public int total { get; set; }
        public List<Dm_Phong_Ban> lst_phong_ban { get; set; }
        public Dm_Phong_Ban phong_ban { get; set; }
        public List<DanhSachNguoiDung> lst_nguoi_dung { get; set; }
    }

    public class DanhSachPhongBanModelV2 : BaseDanhMucModel
    {

        public string ma_phong_ban { get; set; }
        public string ten_phong_ban { get; set; }
        public string mo_ta { get; set; }
        public int? thu_tu { get; set; }
        public Guid nguoi_dai_dien { get; set; }
        public int trang_thai { get; set; }
        public Guid uid { get; set; }
        public int type { get; set; }
        public string type_view { get; set; }
        public string UidName { get; set; }
    }

    public class DataResponse
    {
        public int error { get; set; }
        public string msg { get; set; }
    }

    public class DanhSachNguoiDung
    {
        public Guid id { get; set; }
        public string ten_nguoi_dung { get; set; }
        public string UserName { get; set; }
        public string ma_phong_ban { get; set; }
    }

    public class LuuPhongBan
    {
        public string ma_phong_ban { get; set; }
        public string ten_phong_ban { get; set; }
        public string mo_ta { get; set; }
        public int thu_tu { get; set; }
        public Guid nguoi_dai_dien { get; set; }
        public int trang_thai { get; set; }
        public Guid uid { get; set; }
        public int type { get; set; }
        public string type_view { get; set; }
        public string uid_name { get; set; }
    }
}