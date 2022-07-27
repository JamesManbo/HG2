using HG.Data.SqlService;
using HG.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HG.Data.Business.User
{
    public class UserDao : BaseDAL
    {
        public UserDao(SqlDbProvider dbProvider) : base(dbProvider)
        {

        }

        public List<AspNetUsersModel> GetAspNetUsers()
        {
            DbProvider.SetCommandText2("GetListUser", CommandType.StoredProcedure);

            // Input params
            DbProvider.AddParameter("FullName", 1, SqlDbType.Int);
            // Output params
            //DbProvider.AddParameter("CountRow", DBNull.Value, SqlDbType.NVarChar, 100, ParameterDirection.Output);

            // Lấy về danh sách các trường học
            var obj = DbProvider.ExecuteListObject<AspNetUsersModel>();
            //obj.TotalRecords = int.Parse(DbProvider.Command.Parameters["CountRow"].Value.ToString());
            return obj;
        }
        public UnitsMenuRolesOfUser GetUnitsMenuRolesOfUser(Guid Uid)
        {
            try { 
                var result = new UnitsMenuRolesOfUser();
                DbProvider.SetCommandText2("[GetUnits$Role$User]", CommandType.StoredProcedure);

                // Input params
                DbProvider.AddParameter("Uid", Uid, SqlDbType.UniqueIdentifier);
                DbProvider.ExecuteReader_ToMyReader();
                // Lấy về danh sách các người dung
                result.dm_Phong_Ban = DbProvider.ExecuteReader_frmMyReader<Dm_Phong_Ban>().FirstOrDefault();
                //Lấy về danh sách nhóm
                DbProvider.ExecuteReader_NextResult();
                result.nhom_Vai_Tro = DbProvider.ExecuteReader_frmMyReader<Nhom_Vai_Tro>();
                DbProvider.ExecuteReader_NextResult();
                result.Vai_Tro_Menu_Quyen = DbProvider.ExecuteReader_frmMyReader<HG.Entities.Entities.SuperAdmin.Asp_vaitro_quyen>();
                DbProvider.ExecuteReader_Close();
                return result;
            }catch(Exception e)
            {
                return new UnitsMenuRolesOfUser();
            }
        }

    }
}
