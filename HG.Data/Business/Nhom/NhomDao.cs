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
        //public string ThemMoiNhom(NguoiDungModels item, Guid guid)
        //{
        //    try
        //    {
        //        DbProvider.SetCommandText2("sp_qt_BE_User_Add", CommandType.StoredProcedure);
        //        // Input params
        //        DbProvider.AddParameter("UserName", item.UserName, SqlDbType.NVarChar);
        //        DbProvider.AddParameter("Email", item.Email, SqlDbType.NVarChar);
        //        DbProvider.AddParameter("PhoneNumber", item.PhoneNumber, SqlDbType.NVarChar);
        //        DbProvider.AddParameter("ho_dem", item.ho_dem, SqlDbType.NVarChar);
        //        DbProvider.AddParameter("ten", item.ten, SqlDbType.NVarChar);
        //        DbProvider.AddParameter("anh_dai_dien", item.anh_dai_dien, SqlDbType.NVarChar);
        //        DbProvider.AddParameter("tinh_trang_hon_nhan", item.tinh_trang_hon_nhan, SqlDbType.NVarChar);
        //        DbProvider.AddParameter("ma_phong_ban", item.ma_phong_ban, SqlDbType.NVarChar);
        //        DbProvider.AddParameter("ma_chuc_vu", item.ma_chuc_vu, SqlDbType.NVarChar);
        //        DbProvider.AddParameter("stt", item.stt, SqlDbType.Int);
        //        DbProvider.AddParameter("ngay_sinh", item.ngay_sinh, SqlDbType.DateTime);
        //        // Output params
        //        DbProvider.AddParameter("ma_nguoi_dung_moi", DBNull.Value, SqlDbType.NVarChar, 100, ParameterDirection.Output);
        //        // Lấy về danh sách các người dung
        //        var obj = DbProvider.ExecuteNonQuery();
        //        var NewId = DbProvider.Command.Parameters["ma_nguoi_dung_moi"].Value.ToString();
        //        return NewId == null ? "" : NewId;
        //    }
        //    catch(Exception e)
        //    {
        //        return "";
        //    }
           
        //}
        public void ThemMoi_Nhom(Guid ma_nguoi_dung, string asp_nhom_ma, Guid CreatedUid)
        {
            try
            {
                DbProvider.SetCommandText2("[dbo].[themmoi$nguoidung$nhom]", CommandType.StoredProcedure);
                // Input params
                DbProvider.AddParameter("ma_nguoi_dung", ma_nguoi_dung, SqlDbType.UniqueIdentifier);
                DbProvider.AddParameter("asp_nhom_ma", asp_nhom_ma, SqlDbType.NVarChar);
                DbProvider.AddParameter("CreatedUid", CreatedUid, SqlDbType.UniqueIdentifier);
                // Lấy về danh sách các người dung
                var obj = DbProvider.ExecuteNonQuery();
            }
            catch (Exception e)
            {
             
            }
        }

        public Response XoaNhom(Guid UserId)
		{
            try
            {
                Response result = new Response();
                DbProvider.SetCommandText2("[nguoidung$Xoa]", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("UserId", UserId, SqlDbType.UniqueIdentifier);
                // Output params
                DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.AddParameter("ReturnMsg", DBNull.Value, SqlDbType.NVarChar,1000, ParameterDirection.Output);

                // Lấy về danh sách các người dung
                DbProvider.ExecuteNonQuery();
                result.ErrorCode = Convert.ToInt32(DbProvider.Command.Parameters["ErrCode"].Value.ToString());
                result.ReturnMsg = DbProvider.Command.Parameters["ReturnMsg"].Value.ToString();
                return result;
            }
            catch (Exception e)
            {
                return new Response();
            }
        }
        public Asp_NguoiDung_Nhom LayNhomId(Guid UserId)
        {
            try
            {
                Asp_NguoiDung_Nhom result = new Asp_NguoiDung_Nhom();
                result.responseErr = new Response();
                DbProvider.SetCommandText2("[nguoidung$danhsach$id]", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("UserId", UserId, SqlDbType.UniqueIdentifier);
                // Output params
                DbProvider.AddParameter("ErrCode", DBNull.Value, SqlDbType.Int, ParameterDirection.Output);
                DbProvider.AddParameter("ReturnMsg", DBNull.Value, SqlDbType.NVarChar, 1000, ParameterDirection.Output);
                DbProvider.ExecuteReader_ToMyReader();
                // Lấy về danh sách các người dung
                result.aspNetUsersModel = DbProvider.ExecuteReader_frmMyReader<AspNetUsersModel2>();
                //Lấy về danh sách nhóm
                DbProvider.ExecuteReader_NextResult();
                result.asp_nhom = DbProvider.ExecuteReader_frmMyReader<Asp_nhom>();
                DbProvider.ExecuteReader_Close();
                result.responseErr.ErrorCode = Convert.ToInt32(DbProvider.Command.Parameters["ErrCode"].Value.ToString());
                result.responseErr.ReturnMsg = DbProvider.Command.Parameters["ReturnMsg"].Value.ToString();
                return result;
            }
            catch (Exception e)
            {
                return new Asp_NguoiDung_Nhom();
            }
        }


    }
}
