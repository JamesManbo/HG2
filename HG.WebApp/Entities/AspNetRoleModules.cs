using System.ComponentModel.DataAnnotations.Schema;

namespace HG.WebApp.Entities
{
    [Table("AspNetRoleModules")]
    public class AspNetRoleModules
    {
        public int Id { set; get; }
        public Guid RoleId { set; get; }
        public int ModuleId { set; get; }
        public bool? IsFull { set; get; }
        public bool? IsAdd { set; get; }
        public bool? IsEdit { set; get; }
        public bool? IsDelete { set; get; }

    }
}
