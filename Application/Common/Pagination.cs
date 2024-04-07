
namespace Application.Common;

public static class Pagination
{
    public static IEnumerable<TSource> ToPaged<TSource>(this IEnumerable<TSource> source, int page, int size, out int totalRecords)
    {
        totalRecords = source.Count();
        return source.Skip((page - 1) * size).Take(size);
    }

    public static IQueryable<TSource> ToPaged<TSource>(this IQueryable<TSource> source, int page, int size, out int totalRecords)
    {
        totalRecords = source.Count();
        return source.Skip((page - 1) * size).Take(size);
    }
}
