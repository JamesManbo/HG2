namespace HG.WebApp.Dto
{
    public class NationDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? NameAbbreviation { get; set; }
        public int? Status { get; set; }
        public string? Icon { get; set; }
        public int TotalSum { set; get; }
    }
}
