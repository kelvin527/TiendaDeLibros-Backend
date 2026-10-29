using System.ComponentModel.DataAnnotations;

namespace TiendaDeLibros.Models;

/// <summary>Datos necesarios para actualizar un autor existente.</summary>
public sealed record UpdateAuthorRequest
{
    [Required]
    public required int IdBook { get; init; }

    [Required, MaxLength(100)]
    public required string FirstName { get; init; }

    [Required, MaxLength(100)]
    public required string LastName { get; init; }
}
