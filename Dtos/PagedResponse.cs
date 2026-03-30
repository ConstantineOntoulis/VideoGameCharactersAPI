namespace VideoGameCharacterAPI.Dtos
{
    public class PagedResponseDto<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }  //mtching items of the search
        public List<T> Items { get; set; } = [];  //tells client about page metadata & returns actual characters for the page
    }
}