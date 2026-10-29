using TiendaDeLibros.Models;

namespace TiendaDeLibros.Helpers;

public static class PagingHelper
{
    public const int DefaultPageSize = 10;
    public const int MaxPageSize = 50;

    public static PagedResponse<T> ToPagedResponse<T>(
        IReadOnlyList<T> source, int pageNumber, int pageSize)
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize < 1 ? DefaultPageSize : Math.Min(pageSize, MaxPageSize);

        var totalCount = source.Count;
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var items = source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResponse<T>(items, pageNumber, pageSize, totalCount, totalPages);
    }
}
