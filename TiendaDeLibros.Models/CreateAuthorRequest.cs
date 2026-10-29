using System.ComponentModel.DataAnnotations;

namespace TiendaDeLibros.Models;

/// <summary>Datos necesarios para crear un nuevo autor.</summary>
public sealed record CreateAuthorRequest
{
    [Required]
    public required int IdBook { get; init; }

    [Required, MaxLength(100)]
    public required string FirstName { get; init; }

    [Required, MaxLength(100)]
    public required string LastName { get; init; }
}
