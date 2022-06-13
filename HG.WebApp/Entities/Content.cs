using System.ComponentModel.DataAnnotations.Schema;

namespace HG.WebApp.Entities
{
    [Table("Content")]
    public class Content
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string title_Bold { get; set; }
        public string Des1_Bold { get; set; }
        public string Des1_Normal { get; set; }
        public string Des2_Bold { get; set; }
        public string Des2_Normal { get; set; }
        public string Des3_Bold { get; set; }
        public string Des3_Normal { get; set; }
        public string info_footer1 { get; set; }
        public string info_footer2 { get; set; }
        public string info_footer3 { get; set; }
        public string info_footer4 { get; set; }
        public string info_footer5 { get; set; }
        public string info_footer6 { get; set; }
        public string info_footer7 { get; set; }
        public string info_footer8 { get; set; }
        public string info_footer9 { get; set; }
    }
}
