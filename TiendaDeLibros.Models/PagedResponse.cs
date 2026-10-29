namespace TiendaDeLibros.Models;

/// <summary>Representa una página de resultados.</summary>
public sealed record PagedResponse<T>(
    IReadOnlyList<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages);
