namespace TiendaDeLibros.Models;

/// <summary>Representa un autor devuelto por la API.</summary>
public sealed record AuthorResponse(
    int Id,
    int IdBook,
    string FirstName,
    string LastName) : IIdentifiable;
