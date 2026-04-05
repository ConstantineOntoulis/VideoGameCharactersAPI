namespace VideoGameCharactersAPI.Dtos
{
    public class GetCharactersQuery
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? Game { get; set; }
        public string? Role { get; set; }

        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } = "asc";
    }
}
