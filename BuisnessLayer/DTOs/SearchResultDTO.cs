namespace BusinessLayer.DTOs
{
    public class SearchResultDTO
    {
        public string Id { get; set; } // TConst or NConst
        public string Type { get; set; } // "Title" or "Person"
        public string Name { get; set; }
    }
}
