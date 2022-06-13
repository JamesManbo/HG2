namespace HG.WebApp.Dto
{
    public class CategoryDTO
    {
        public int Id { set; get; }
        public string? Name { set; get; }
        public string? NameEn { set; get; }
        public string? Description { set; get; }
        public int? SortOrder { set; get; }
        public int? IsShowOnHome { set; get; }
        public string? ImagePath { set; get; }
        public int? ParentId { set; get; }
        public int Status { set; get; }
        public int TotalSum { set; get; }
       
    }
}
