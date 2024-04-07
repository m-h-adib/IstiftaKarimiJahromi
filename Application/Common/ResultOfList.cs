

namespace Application.Common;

public class ResultOfList<TResource>
{
    public ICollection<TResource>? Items { get; set; }
    public Paging? Paging { get; set; }
}
