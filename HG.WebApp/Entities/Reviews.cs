namespace HG.WebApp.Entities
{
    public class Reviews
    {
        public int Id { set; get; }
        public int ListtingID { set; get; }
        public string? Title { set; get; }
        public string? Comment { set; get; }
        public string? Description { set; get; }
        public int? Rating { set; get; }
        public DateTime? Date { set; get; }
        public DateTime? DateModify { set; get; }
        public int? CustomerID { set; get; }
        public string? AspNetUserID { set; get; }
        public int? Status { set; get; }
    }
}
