namespace TiendaDeLibros.Models;

/// <summary>Representa un libro devuelto por la API.</summary>
public sealed record BookResponse(
    int Id,
    string Title,
    string? Description,
    int PageCount,
    string? Excerpt,
    DateTimeOffset PublishDate) : IIdentifiable;
