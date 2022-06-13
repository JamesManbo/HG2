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


    }
}
