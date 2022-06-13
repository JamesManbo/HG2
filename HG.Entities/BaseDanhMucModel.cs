using System.ComponentModel.DataAnnotations.Schema;

namespace HG.Entities
{
    public class BaseDanhMucModel
    {
        public int? Status { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
        public Guid? CreatedUid { get; set; }
        public DateTime? UpdatedDateUtc { get; set; }
        public Guid? UpdatedUid { get; set; }
        public int? Deleted { get; set; }
        public Guid? DeletedBy { get; set; }
        public int? Stt { get; set; }
        public string? UidName { get; set; }
        [NotMapped]
        public string? type_view { get; set; }
    }
}