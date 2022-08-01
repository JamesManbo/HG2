using HG.Data.SqlService;
using HG.Entities;
using HG.Entities.Entities;
using HG.Entities.Entities.Luong;
using HG.Entities.HoSo;
using HG.Entities.SearchModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HG.Data.Business.HoSo
{
    public class XuLyHoSoDao : BaseDAL
    {
        public XuLyHoSoDao(SqlDbProvider dbProvider) : base(dbProvider)
        {
           
        }
        public List<QuyTrinhXuLy> DanhSachQuyTrinhXuLyKey()
        {
            try
            {
                DbProvider.SetCommandText2("Get_quytrinh_xuly", CommandType.StoredProcedure);
                // Lấy về danh sách các người dung
                var menu = DbProvider.ExecuteListObject<QuyTrinhXuLy>();
                return menu;
            }
            catch (Exception e)
            {
                return new List<QuyTrinhXuLy>();
            }
        }
        

    }
}
