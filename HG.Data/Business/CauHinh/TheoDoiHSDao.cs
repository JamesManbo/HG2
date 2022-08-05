using HG.Data.SqlService;
using HG.Entities;
using HG.Entities.Entities;
using HG.Entities.Entities.CauHinh;
using HG.Entities.Entities.Nhom;
using HG.Entities.SearchModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Data.Business.CauHinh
{
    public class TheoDoiHSDao : BaseDAL
    {
        public TheoDoiHSDao(SqlDbProvider dbProvider) : base(dbProvider)
        {

        }

        public List<cd_theo_doi_hs> HoSoSapHetHan(string from, string to, int page, int page_size, out int total)
        {
            total = 0;
            try
            {
              
                List<cd_theo_doi_hs> nguoi_PHXLModel = new List<cd_theo_doi_hs>();
                DbProvider.SetCommandText2("Proc_DanhSachHoSo", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("@from", from, SqlDbType.DateTime);
                DbProvider.AddParameter("@to", to, SqlDbType.DateTime);
                DbProvider.AddParameter("@page", page, SqlDbType.Int);
                DbProvider.AddParameter("@page_size", page_size, SqlDbType.Int);

                // Output params
                DbProvider.AddParameter("total", DBNull.Value, SqlDbType.Int, 100, ParameterDirection.Output);
                nguoi_PHXLModel = DbProvider.ExecuteListObject<cd_theo_doi_hs>();
                total = Convert.ToInt32(DbProvider.Command.Parameters["total"].Value.ToString());

                return nguoi_PHXLModel;
            }
            catch (Exception e)
            {
                return new List<cd_theo_doi_hs>();
            }
        }

        public List<cd_theo_doi_hs> HoSoSapHetHanExcel(string from, string to)
        {
           
            try
            {

                List<cd_theo_doi_hs> nguoi_PHXLModel = new List<cd_theo_doi_hs>();
                DbProvider.SetCommandText2("Proc_DanhSachHoSo_excel", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("@from", from, SqlDbType.DateTime);
                DbProvider.AddParameter("@to", to, SqlDbType.DateTime);
     
                nguoi_PHXLModel = DbProvider.ExecuteListObject<cd_theo_doi_hs>();  
                return nguoi_PHXLModel;
            }
            catch (Exception e)
            {
                return new List<cd_theo_doi_hs>();
            }
        }

        public List<cd_theo_doi_hs> HoSoSapChamTiepNhan(string from, string to, int page, int page_size, out int total)
        {
            total = 0;
            try
            {

                List<cd_theo_doi_hs> nguoi_PHXLModel = new List<cd_theo_doi_hs>();
                DbProvider.SetCommandText2("Proc_DanhSachHoSoChamTiepNhan", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("@from", from, SqlDbType.DateTime);
                DbProvider.AddParameter("@to", to, SqlDbType.DateTime);
                DbProvider.AddParameter("@page", page, SqlDbType.Int);
                DbProvider.AddParameter("@page_size", page_size, SqlDbType.Int);

                // Output params
                DbProvider.AddParameter("total", DBNull.Value, SqlDbType.Int, 100, ParameterDirection.Output);
                nguoi_PHXLModel = DbProvider.ExecuteListObject<cd_theo_doi_hs>();
                total = Convert.ToInt32(DbProvider.Command.Parameters["total"].Value.ToString());

                return nguoi_PHXLModel;
            }
            catch (Exception e)
            {
                return new List<cd_theo_doi_hs>();
            }
        }

        public List<cd_theo_doi_hs> HoSoSapChamTiepNhannExcel(string from, string to)
        {

            try
            {

                List<cd_theo_doi_hs> nguoi_PHXLModel = new List<cd_theo_doi_hs>();
                DbProvider.SetCommandText2("Proc_DanhSachHoSoChamTiepNhan_excel", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("@from", from, SqlDbType.DateTime);
                DbProvider.AddParameter("@to", to, SqlDbType.DateTime);

                nguoi_PHXLModel = DbProvider.ExecuteListObject<cd_theo_doi_hs>();
                return nguoi_PHXLModel;
            }
            catch (Exception e)
            {
                return new List<cd_theo_doi_hs>();
            }
        }

    }
}
