using System.ComponentModel.DataAnnotations.Schema;

namespace HG.WebApp.Entities
{
    [Table("AspModule")]
    public class AspModule
    {
        public int Id { set; get; }
        public string Name { get; set; }
        public string link { get; set; }
        public int isEF { get; set; } //1 la FE 2 la Ad
        public string? Logo { get; set; }
        public int Status { get; set; }
    }
}
