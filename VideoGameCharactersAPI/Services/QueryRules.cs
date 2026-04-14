namespace VideoGameCharactersAPI.Services
{
    public class QueryRules
    {
        //This method receives a page number and returns the corrected page number
        public static int NormalizePage(int page)
        {
            //If the requested page is smaller than 1, use page 1 instead
            return page < 1 ? 1 : page;
        }
        //If the requested page size is smaller than 1, use 10. If it is too large, cap it at 50
        public static int NormalizePageSize(int pageSize)
        {
            //This method receives a page size and returns the corrected page size
            return pageSize < 1 ? 10 : Math.Min(pageSize, 50);
        }
    }
}
