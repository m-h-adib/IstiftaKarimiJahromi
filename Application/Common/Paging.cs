

namespace Application.Common;

public class Paging
{
    public Paging()
    { }

    public Paging(int page, int size, int totalItems)
    {
        Page = page;
        Size = size;
        TotalItems = totalItems;
    }

    public int Page { get; set; }
    public int Size { get; set; }
    public int TotalItems { get; set; }
}
