namespace HG.WebApp.Entities
{
    public class Slider
    {
        public int Id { get; set; }
        public int ListtingId { get; set; }
        public string? SliderName { get; set; }
        public string? SliderUrl { get; set; }
        public string? Note { get; set; }
        public int? IsPartner { set; get; }
        public int? Status { set; get; }
    }
}
