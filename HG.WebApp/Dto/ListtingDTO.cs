using HG.WebApp.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HG.WebApp.Dto
{
    public class ListtingDTO
    {
     
            public int Id { set; get; }
            public string? Title { set; get; }
            public string? TitleSeo { set; get; }
            public string? Phone { set; get; }
            public string? Gmail { set; get; }
            public string? Postal { set; get; }
            public string? CurrentMap { set; get; }
            public string? Description { set; get; }
            public string? WebSite { set; get; }
            [Column(TypeName = "datetime2")]
            [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
            public DateTime? Date { set; get; } = null;
            [Column(TypeName = "datetime2")]
            [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
            public DateTime? DateCreate { set; get; } = null;
            [Column(TypeName = "datetime2")]
            [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
            public DateTime? DateModify { set; get; } = null;
            public string? UserUpdate { set; get; }
            public string? Claim { set; get; }
            public int IsComment { set; get; }
            public int? Status { set; get; }
            public string? Feature { set; get; }
            public string? CategorysString { set; get; }
            public Guid? UserID { set; get; }
        //CeoInformation

        public int CustomerCeosID { set; get; }
            public int CategorysID { set; get; }

            public int ListtingImageID { set; get; }
            public int? NationsID { set; get; }

            public CustomerCeo customerCeo { set; get; }
            public ListtingImage ListtingImage { set; get; }
            public Location Locations { set; get; }


    }
}
