using HG.Data.SqlService;
using HG.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Data.Business.Sys
{
    public class SystemDao : BaseDAL
    {
        public SystemDao(SqlDbProvider dbProvider) : base(dbProvider)
        {

        }

        public string GetMaDonViByUserId(Guid userId)
        {
            try
            {
                DbProvider.SetCommandText2("GetMaDonViByUserId", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("@userId", userId, SqlDbType.UniqueIdentifier);
                // Output params
                DbProvider.AddParameter("ma_don_vi", DBNull.Value, SqlDbType.VarChar, 100, ParameterDirection.Output);
                DbProvider.ExecuteNonQuery();
                var ma_don_vi = DbProvider.Command.Parameters["ma_don_vi"].Value.ToString();
                return ma_don_vi;
            }
            catch (Exception e)
            {
                return "";
            }
            
        }
        public string GetMaPhongBanByUserId(Guid userId)
        {
            try
            {
                DbProvider.SetCommandText2("GetMaPhongBanByUserId", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("@userId", userId, SqlDbType.UniqueIdentifier);
                // Output params
                DbProvider.AddParameter("ma_phong_ban", DBNull.Value, SqlDbType.VarChar, 100, ParameterDirection.Output);
                DbProvider.ExecuteNonQuery();
                var ma_phong_ban = DbProvider.Command.Parameters["ma_phong_ban"].Value.ToString();
                return ma_phong_ban;
            }
            catch (Exception e)
            {
                return "";
            }
        }
      
        public Boolean LaAdminPhongBan(Guid userId)
        {
            try
            {
                DbProvider.SetCommandText2("LaAdminPhongBan", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("@userId", userId, SqlDbType.UniqueIdentifier);
                // Output params
                DbProvider.AddParameter("isAdminPB", DBNull.Value, SqlDbType.Int, 100, ParameterDirection.Output);
                DbProvider.ExecuteNonQuery();
                var isAdminPB = Convert.ToInt32(DbProvider.Command.Parameters["isAdminPB"].Value.ToString());
                if (isAdminPB == 1)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public Boolean LaAdminDonVi(Guid userId)
        {
            try
            {
                DbProvider.SetCommandText2("LaAdminDonVi", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("@userId", userId, SqlDbType.UniqueIdentifier);
                // Output params
                DbProvider.AddParameter("isAdminPB", DBNull.Value, SqlDbType.Int, 100, ParameterDirection.Output);
                DbProvider.ExecuteNonQuery();
                var isAdminPB = Convert.ToInt32(DbProvider.Command.Parameters["isAdminPB"].Value.ToString());
                if(isAdminPB == 1)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
