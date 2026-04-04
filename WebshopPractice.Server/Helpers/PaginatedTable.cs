namespace WebshopPractice.Server.Helpers;

public class PaginatedTable<T>
{
    //number of the currently represented page
    public int PageNumber { get; set; }

    //number of total pages
    public int PageCount { get; set; }

    //number of rows on the current page
    public int PageSize { get; set; }

    //total number of records
    public int TotalRecordCount { get; set; }

    //the actual rows currently represented in the table
    public IEnumerable<T> Body { get; set; } = [];
}
