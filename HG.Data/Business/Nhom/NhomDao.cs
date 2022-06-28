using HG.Data.SqlService;
using HG.Entities;
using HG.Entities.Entities;
using HG.Entities.Entities.Nhom;
using HG.Entities.SearchModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Data.Business.Nhom
{
    public class NhomDao : BaseDAL
    {
        public NhomDao(SqlDbProvider dbProvider) : base(dbProvider)
        {

        }

        public Asp_nhom_paging LayDsNhomPhanTrang(NhomSearchItem item)
        {
            try
            {
                Asp_nhom_paging asp_Nhom_Paging = new Asp_nhom_paging();
                asp_Nhom_Paging.Pagelist = new Pagelist();
                DbProvider.SetCommandText2("nhom$danhsanh$phantrang", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("StartingRow", item.PageIndex, SqlDbType.Int);
                DbProvider.AddParameter("RecordsPerPage", item.RecordsPerPage, SqlDbType.Int);
                DbProvider.AddParameter("tu_khoa", "", SqlDbType.NVarChar);
                // Output params
                DbProvider.AddParameter("tong_ban_ghi", DBNull.Value, SqlDbType.Int, 100, ParameterDirection.Output);

                // Lấy về danh sách các người dung
                asp_Nhom_Paging.asp_Nhoms = DbProvider.ExecuteListObject<Asp_nhom>();
                asp_Nhom_Paging.Pagelist.TotalRecords = Convert.ToInt32(DbProvider.Command.Parameters["tong_ban_ghi"].Value.ToString());
                return asp_Nhom_Paging;
            }
            catch(Exception e) {
                return new Asp_nhom_paging();  }
        }
        public Response ThemMoiNhom(NhomModel item, Guid guid)
        {
            try
            {
                Response response = new Response();
                DbProvider.SetCommandText2("nhom$themmoi", CommandType.StoredProcedure);
                // Input params
                DbProvider.AddParameter("ma_nhom", item.ma_nhom, SqlDbType.NVarChar);
                DbProvider.AddParameter("ten_nhom", item.ten_nhom, SqlDbType.NVarChar);
                DbProvider.AddParameter("mo_ta", item.mo_ta, SqlDbType.NVarChar);
                DbProvider.AddParameter("stt", item.stt, SqlDbType.Int);
                DbProvider.AddParameter("userId", guid, SqlDbType.UniqueIdentifier);
       
                // Output params
                DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.AddParameter("ReturnMsg", DBNull.Value, SqlDbType.NVarChar, 100, ParameterDirection.Output);
                // Lấy về danh sách các người dung
                DbProvider.ExecuteNonQuery();
                response.ErrorCode = Convert.ToInt32(DbProvider.Command.Parameters["ErrCode"].Value.ToString());
                response.ReturnMsg = DbProvider.Command.Parameters["ReturnMsg"].Value.ToString();
                return response;
            }
            catch (Exception e)
            {
                return new Response() { ErrorCode = 1, ReturnMsg = "Lỗi hệ thống" };
            }

        }

        public Response ChinhSuaNhom(NhomModel item)
        {
            try
            {
                Response response = new Response();
                DbProvider.SetCommandText2("nhom$chinhsua", CommandType.StoredProcedure);
                // Input params
                DbProvider.AddParameter("ma_nhom", item.ma_nhom, SqlDbType.NVarChar);
                DbProvider.AddParameter("ten_nhom", item.ten_nhom, SqlDbType.NVarChar);
                DbProvider.AddParameter("mo_ta", item.mo_ta, SqlDbType.NVarChar);
                DbProvider.AddParameter("stt", item.stt, SqlDbType.Int);
                DbProvider.AddParameter("userId", item.UpdatedUid, SqlDbType.UniqueIdentifier);

                // Output params
                DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.AddParameter("ReturnMsg", DBNull.Value, SqlDbType.NVarChar, 100, ParameterDirection.Output);
                // Lấy về danh sách các người dung
                DbProvider.ExecuteNonQuery();
                response.ErrorCode = Convert.ToInt32(DbProvider.Command.Parameters["ErrCode"].Value.ToString());
                response.ReturnMsg = DbProvider.Command.Parameters["ReturnMsg"].Value.ToString();
                return response;
            }
            catch (Exception e)
            {
                return new Response() { ErrorCode = 1, ReturnMsg = "Lỗi hệ thống" };
            }

        }
        public int XoaNhom(string ma_phong_ban, Guid uid)
        {
            DbProvider.SetCommandText2("nhom$xoa", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_nhom", ma_phong_ban, SqlDbType.VarChar);
            DbProvider.AddParameter("uid", uid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            //ma_loi = int.Parse(DbProvider.Command.Parameters["total"].Value.ToString());
            var ma_loi = Convert.ToInt32(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }

        public NhomModel LayNhomId(Guid NhomId)
        {
            try
            {
                NhomModel result = new NhomModel();
                DbProvider.SetCommandText2("[nhom$danhsach$id]", CommandType.StoredProcedure);
                // Input params
                DbProvider.AddParameter("NhomId", NhomId, SqlDbType.UniqueIdentifier);
                result = DbProvider.ExecuteObject<NhomModel>();
                return result;
            }
            catch (Exception e)
            {
                return new NhomModel();
            }
        }

        public int XoaQuyen(string maquyen, Guid uid)
        {
            DbProvider.SetCommandText2("quyen$xoaquyen", CommandType.StoredProcedure);
            DbProvider.AddParameter("ma_quyen", maquyen, SqlDbType.VarChar);
            DbProvider.AddParameter("uid", uid, SqlDbType.UniqueIdentifier);
            DbProvider.AddParameter("ma_loi", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteNonQuery();
            //ma_loi = int.Parse(DbProvider.Command.Parameters["total"].Value.ToString());
            var ma_loi = Convert.ToInt32(DbProvider.Command.Parameters["ma_loi"].Value.ToString());
            return ma_loi;
        }
    }
}
