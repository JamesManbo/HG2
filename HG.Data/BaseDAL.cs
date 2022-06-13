using HG.Data.SqlService;

namespace HG.Data
{
    public class BaseDAL
    {
        /// <summary>
        /// Khởi tạo constructor
        /// </summary>
        /// <param name="dbProvider"></param>
        public BaseDAL(SqlDbProvider dbProvider)
        {
            DbProvider = dbProvider;
        }

        /// <summary>
        /// Share properties SqlDbProvider
        /// </summary>
        protected SqlDbProvider DbProvider { get; set; }
    }
}
